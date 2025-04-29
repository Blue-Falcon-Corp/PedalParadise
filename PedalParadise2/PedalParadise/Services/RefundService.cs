namespace PedalParadise.Services
{
    using PedalParadise.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PedalParadise.Data;

    public class RefundService : IRefundService
    {
        private readonly PedalParadiseContext _refunds;

        public RefundService(PedalParadiseContext refunds)
        {
            _refunds = refunds;
        }

        // Get all refund requests
        public async Task<IEnumerable<RefundRequest>> GetAllRefundsAsync()
        {
            return await _refunds.RefundRequests.Include(r => r.Order).ToListAsync();
        }

        // Get a refund request by ID
        public async Task<RefundRequest?> GetRefundByIdAsync(int id)
        {
            return await _refunds.RefundRequests.Include(r => r.Order)
                                                .FirstOrDefaultAsync(r => r.RefundID == id);
        }
        //gets refunds by client
        public async Task<IEnumerable<RefundRequest>> GetRefundsByClientIdAsync(int clientId)
        {
            return await _refunds.RefundRequests
                .Include(r => r.Order) // Ensure Order is loaded
                .ThenInclude(o => o.UserID) // Access Client through Order
                .Where(r => r.Order.UserID == clientId) // Filter by ClientID from Order
                .ToListAsync();
        }

        // Create a new refund request
        public async Task<RefundRequest> CreateRefundRequestAsync(RefundRequest refund)
        {
            _refunds.RefundRequests.Add(refund);
            await _refunds.SaveChangesAsync();
            return refund;
        }

        // Update the status of a refund request
        public async Task<bool> UpdateRefundStatusAsync(int refundId, string status)
        {
            var refundRequest = await _refunds.RefundRequests.FindAsync(refundId);
            if (refundRequest == null) return false;

            refundRequest.Status = status;
            _refunds.RefundRequests.Update(refundRequest);
            await _refunds.SaveChangesAsync();

            return true;
        }
    }
}
