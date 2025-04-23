using PedalParadise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepairService
{
    Task<IEnumerable<RepairRequest>> GetAllRepairsAsync();
    Task<RepairRequest?> GetRepairByIdAsync(int id);
    Task<IEnumerable<RepairRequest>> GetRepairsByClientIdAsync(int clientId);
    Task<IEnumerable<RepairRequest>> GetRepairsByEmployeeIdAsync(int employeeId);
    Task<RepairRequest> CreateRepairRequestAsync(RepairRequest repair);
    Task<bool> UpdateRepairStatusAsync(int repairId, string status);
    Task<bool> AssignRepairToEmployeeAsync(int repairId, int employeeId);
    Task<bool> DeleteRepairRequestAsync(int id);
}