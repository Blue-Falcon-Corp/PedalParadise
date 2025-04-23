using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models;
using PedalParadise.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PedalParadise.Models.ViewModels;

namespace PedalParadise.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Cart
        [Route("/Cart")]
        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var products = new List<CartViewModel>();

            foreach (var item in cart)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductID);
                if (product != null)
                {
                    products.Add(new CartViewModel
                    {
                        ProductID = product.ProductID,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            return View(products);
        }

        // POST: /Cart/AddToCart
        [Route("/Cart/Add")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(i => i.ProductID == id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { ProductID = id, Quantity = quantity });
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/UpdateQuantity
        [Route("Cart/UpdateQuantity")]
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            if (quantity <= 0)
            {
                return RedirectToAction(nameof(RemoveFromCart), new { id });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductID == id);

            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/RemoveFromCart
        [Route("/Cart/RemoveFromCart")]
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductID == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper methods
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        // GET: /Cart/Checkout
        [Route("/Cart/Checkout")]
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Cart") });
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}