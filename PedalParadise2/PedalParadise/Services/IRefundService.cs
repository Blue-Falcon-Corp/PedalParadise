using PedalParadise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRefundService
{
    Task<IEnumerable<RefundRequest>> GetAllRefundsAsync();
    Task<RefundRequest?> GetRefundByIdAsync(int id);
    Task<IEnumerable<RefundRequest>> GetRefundsByClientIdAsync(int clientId);
    Task<RefundRequest> CreateRefundRequestAsync(RefundRequest refund);
    Task<bool> UpdateRefundStatusAsync(int refundId, string status);
}