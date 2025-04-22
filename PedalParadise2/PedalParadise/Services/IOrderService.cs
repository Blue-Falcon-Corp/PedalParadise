using PedalParadise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace PedalParadise.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);  // Changed to Order?
        Task<IEnumerable<Order?>> GetOrdersByClientIdAsync(int clientId);
        Task<Order> CreateOrderAsync(Order order, List<CartItem> cartItems);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> DeleteOrderAsync(int id);
    }
}