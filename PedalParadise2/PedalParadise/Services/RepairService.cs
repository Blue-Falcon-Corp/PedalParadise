// Services/RepairService.cs
using Microsoft.EntityFrameworkCore;
using PedalParadise.Data;
using PedalParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PedalParadise.Services
{
    public class RepairService : IRepairService
    {
        private readonly PedalParadiseContext _context;
        public RepairService(PedalParadiseContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RepairRequest>> GetAllRepairsAsync()
        {
            return await _context.RepairRequests
                .Include(r => r.Client)
                    .ThenInclude(c => c!.User) // Add null-forgiving operator
                .Include(r => r.AssignedEmployee)
                    .ThenInclude(e => e!.User) // Add null-forgiving operator
                .ToListAsync();
        }
        public async Task<RepairRequest?> GetRepairByIdAsync(int id)
        {
            return await _context.RepairRequests
                .Include(r => r.Client)
                    .ThenInclude(c => c!.User) // Add null-forgiving operator
                .Include(r => r.AssignedEmployee)
                    .ThenInclude(e => e!.User) // Add null-forgiving operator
                .FirstOrDefaultAsync(r => r.RepairID == id);
        }
        public async Task<IEnumerable<RepairRequest>> GetRepairsByClientIdAsync(int clientId)
        {
            return await _context.RepairRequests
                .Include(r => r.AssignedEmployee)
                    .ThenInclude(e => e!.User) // Add null-forgiving operator
                .Where(r => r.ClientID == clientId)
                .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<RepairRequest>> GetRepairsByEmployeeIdAsync(int employeeId)
        {
            return await _context.RepairRequests
                .Include(r => r.Client)
                    .ThenInclude(c => c!.User) // Add null-forgiving operator
                .Where(r => r.AssignedEmployeeID == employeeId)
                .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();
        }
        public async Task<RepairRequest> CreateRepairRequestAsync(RepairRequest repair)
        {
            _context.RepairRequests.Add(repair);
            await _context.SaveChangesAsync();
            return repair;
        }
        public async Task<bool> UpdateRepairStatusAsync(int repairId, string status)
        {
            var repair = await _context.RepairRequests.FindAsync(repairId);
            if (repair == null)
            {
                return false;
            }
            repair.Status = status;
            if (status == "Completed")
            {
                repair.CompletedDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AssignRepairToEmployeeAsync(int repairId, int employeeId)
        {
            var repair = await _context.RepairRequests.FindAsync(repairId);
            if (repair == null)
            {
                return false;
            }
            repair.AssignedEmployeeID = employeeId;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRepairRequestAsync(int id)
        {
            var repair = await _context.RepairRequests.FindAsync(id);
            if (repair == null)
            {
                return false;
            }
            _context.RepairRequests.Remove(repair);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}