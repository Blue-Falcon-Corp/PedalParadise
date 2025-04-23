// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models;
using PedalParadise.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PedalParadise.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Products
        [Route("/Products")]
        public async Task<IActionResult> Index(string category, string search, string sortOrder)
        {
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentSort"] = sortOrder;

            var products = await _productService.GetAllProductsAsync();

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // Apply search
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default: // "name_asc"
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            return View(products);
        }

        // GET: /Products/Details/5
        [Route("/Products/Details/id")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: /Products/Create (Admin only)
        [Route("Products/Create")]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /Products/Create
        [Route("Products/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: /Products/Edit/5 (Admin only)
        [Route("Products/Edit/id")]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Edit/5
        [Route("Products/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: /Products/Delete/5 (Admin only)
        [Route("Products/Delete/id")]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Products/Delete/5
        [Route("Products/Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
