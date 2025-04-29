// Services/ProductService.cs
using Microsoft.EntityFrameworkCore;
using PedalParadise.Data;
using PedalParadise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PedalParadise.Services
{
    public class ProductService : IProductService
    {
        private readonly PedalParadiseContext _context;

        public ProductService(PedalParadiseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        /*public async Task<Product> GetProductByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.ProductID == id);
#pragma warning restore CS8603 // Possible null reference return.
        }*/

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Client) // No need for `.ThenInclude(c => c.User)`
                .FirstOrDefaultAsync(p => p.ProductID == id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count)
        {
            // In a real app, you might have a "Featured" flag or use other criteria
            return await _context.Products
                .OrderByDescending(p => p.ProductID) // Just getting newest products as featured
                .Take(count)
                .ToListAsync();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}