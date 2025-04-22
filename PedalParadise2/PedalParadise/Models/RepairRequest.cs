using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class RepairRequest
    {
        [Key]
        public int RepairID { get; set; }

        [Required]
        public int ClientID { get; set; }

        [Required]
        [StringLength(200)]
        public string BikeDetails { get; set; } = string.Empty;

        [Required]
        public string IssueDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Pending, Diagnosis, PartsOrdered, InProgress, Completed, Closed

        public int? AssignedEmployeeID { get; set; }

        [Required]
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }

        public virtual Employee? AssignedEmployee { get; set; }
    }
}