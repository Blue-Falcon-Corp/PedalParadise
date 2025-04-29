using Microsoft.EntityFrameworkCore;
using PedalParadise.Data;
using PedalParadise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PedalParadise.Services
{
    public class OrderService : IOrderService
    {
        private readonly PedalParadiseContext _context;

        public OrderService(PedalParadiseContext context)
        {
            _context = context;
        }

        /*public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .ThenInclude(c => c.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .ThenInclude(c => c.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.PaymentMethod)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }*/

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client) // Client is still valid, but no need for `.ThenInclude(c => c.User)`
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Client) // No need to reference User explicitly
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.PaymentMethod)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserID == clientId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order, List<CartItem> cartItems)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Add the order
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add order items from cart items
                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity
                    };

                    _context.OrderItems.Add(orderItem);

                    // Update product stock
                    var product = await _context.Products.FindAsync(item.ProductID);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        _context.Entry(product).State = EntityState.Modified;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}