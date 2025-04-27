using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalParadise.Models.ViewModels;
using PedalParadise.Models;
using PedalParadise.Data;

namespace PedalParadise.Controllers;

public class CheckoutController : Controller
{
    private readonly PedalParadiseContext _context;

    public CheckoutController(PedalParadiseContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        //int clientId = 11;
        var clientId = HttpContext.Session.GetInt32("UserId");
        if (clientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.ClientID == clientId.Value);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("EmptyCart");
        }

        decimal total = cart.CartItems.Sum(item => item.Product!.Price * item.Quantity);

        //var paymentMethods = await _context.PaymentMethods.ToListAsync();

        var viewModel = new CheckoutViewModel
        {
            Cart = cart,
            TotalAmount = total
        };

        return View(viewModel);
    }

    public IActionResult EmptyCart() { return View(); }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(CheckoutViewModel viewModel)
    {
        var clientId = HttpContext.Session.GetInt32("UserId");

        if (clientId == null)
        {
            return RedirectToAction("login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", viewModel);
        }

        //int clientId = 11;

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.ClientID == clientId.Value);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("EmptyCart");
        }

        decimal totalAmount = cart.CartItems.Sum(item => item.Product!.Price * item.Quantity);

        var newOrder = new Order
        {
            Date = DateTime.Now,
            ClientID = clientId.Value,
            TotalAmount = totalAmount,
            Status = "Processing",
            PaymentID = null
        };

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        foreach (var item in cart.CartItems)
        {
            var orderItem = new OrderItem
            {
                OrderID = newOrder.OrderID,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Product!.Price
            };

            _context.OrderItems.Add(orderItem);
        }

        _context.CartItems.RemoveRange(cart.CartItems);
        _context.ShoppingCarts.Remove(cart);

        await _context.SaveChangesAsync();

        return RedirectToAction("Success", new { orderId = newOrder.OrderID });
    }

    public async Task<IActionResult> Success(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderID == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
}
