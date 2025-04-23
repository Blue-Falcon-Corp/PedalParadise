using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class RefundRequest
    {
        [Key]
        public int RefundID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public string Reason { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ProcessedDate { get; set; }
        // Navigation properties
        public virtual Order? Order { get; set; }
    }

}