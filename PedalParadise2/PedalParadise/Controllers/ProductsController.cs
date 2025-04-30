// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models;
using PedalParadise.Models.ViewModels;
using PedalParadise.Services;


namespace PedalParadise.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductsController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
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
        [Route("Products/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
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

            if (id != product.ProductID)
            {
                return NotFound();
            }
            // Get all model properties with values(filter out null ones)
            var providedProperties = typeof(Product).GetProperties()
                .Where(p => p.GetValue(product) != null)
                .Select(p => p.Name)
                .ToList();

            // Remove validation for properties not in the request
            foreach (var property in ModelState.Keys.ToList())
            {
                if (!providedProperties.Contains(property))
                {
                    ModelState.Remove(property);
                }
            }

            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: /Products/Delete/5 (Admin only)
        [Route("Products/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
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
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: List all products
        public async Task<IActionResult> List()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserByIdAsync(userId.Value);

            var profileModel = new ProfileViewModel { User = user };
            var products = await _productService.GetAllProductsAsync();

            var viewModel = new ProductsPageViewModel
            {
                Profile = profileModel,
                Products = (List<Product>)products
            };

            return View(viewModel);
        }

    }

}
