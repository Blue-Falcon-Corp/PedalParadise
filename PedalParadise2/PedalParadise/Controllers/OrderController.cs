using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models;
using PedalParadise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PedalParadise.Models.ViewModels;

namespace PedalParadise.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService orderService, IProductService productService, IUserService userService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
        }

        // GET: /Orders/History
        [Route("/Order/History")]
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // getting args for viewModel
            var user = await _userService.GetUserByIdAsync(userId.Value);
            var orders = await _orderService.GetOrdersByClientIdAsync(userId.Value);

            var viewModel = new ProfileViewModel
            {
                User = user,
                Orders = (List<Order>)orders
            };

            return View(viewModel);
        }

        // GET: /Orders/Details/5
        [Route("Orders/Details/")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Ensure user can only see their own orders unless they're an employee
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee" && order.UserID != userId)
            {
                return Forbid();
            }

            return View(order);
        }

        // POST: /Orders/Checkout
        [Route("/Orders/Checkout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var cartJson = HttpContext.Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return RedirectToAction("Index", "Cart");
                }

                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                if (cartItems == null || cartItems.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                // Calculate total
                decimal total = 0;
                foreach (var item in cartItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductID);
                    if (product != null)
                    {
                        total += product.Price * item.Quantity;
                    }
                }

                // Create order
                var order = new Order
                {
                    UserID = userId.Value, // Assuming UserID equals UserID for simplicity
                    Date = DateTime.Now,
                    TotalAmount = total,
                    Status = "Processing",
                    // In a real app, you'd create a PaymentMethod record and associate it
                };

                // Create the order and order items - get the created order back
                var createdOrder = await _orderService.CreateOrderAsync(order, cartItems);

                // Clear the cart
                HttpContext.Session.Remove("Cart");

                return RedirectToAction(nameof(Confirmation), new { id = createdOrder.OrderID });
            }

            return View(model);
        }

        // GET: /Orders/Confirmation/5
        [Route("/Orders/Confirmation/")]
        public async Task<IActionResult> Confirmation(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: /Orders/Cancel/5
        [Route("/Orders/Cancel/")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow cancellation of processing orders
            if (order.Status != "Processing")
            {
                TempData["ErrorMessage"] = "Only orders in 'Processing' status can be cancelled.";
                return RedirectToAction(nameof(Details), new { id = order.OrderID });
            }

            return View(order);
        }

        // POST: /Orders/CancelConfirmed/5
        [Route("/Orders/CancelConfirmed/")]
        [HttpPost, ActionName("CancelConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow cancellation of processing orders
            if (order.Status != "Processing")
            {
                TempData["ErrorMessage"] = "Only orders in 'Processing' status can be cancelled.";
                return RedirectToAction(nameof(Details), new { id = order.OrderID });
            }

            await _orderService.UpdateOrderStatusAsync(id, "Cancelled");
            TempData["SuccessMessage"] = "Order successfully cancelled.";

            return RedirectToAction(nameof(History));
        }
        //GET: Orders list
        [Route("/Order/OrdersList")]
        public async Task<IActionResult> OrdersList()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserByIdAsync(userId.Value);

            var profileModel = new ProfileViewModel { User = user };
            var orders = await _orderService.GetAllOrdersAsync();

            var viewModel = new OrderPageViewModel
            {
                Profile = profileModel,
                Orders = (List<Order>)orders
            };

            return View(viewModel);
        }
    }
}