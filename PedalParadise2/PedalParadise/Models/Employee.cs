using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("User")]
        public int EmployeeID { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Salary { get; set; }
        public int? Manager { get; set; }
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Employee? ManagerNavigation { get; set; }
        public virtual ICollection<Employee> ManagedEmployees { get; set; } = new List<Employee>();
        public virtual ICollection<RepairRequest> AssignedRepairs { get; set; } = new List<RepairRequest>();
    }
}