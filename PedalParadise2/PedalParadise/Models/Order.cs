using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    [Table("Order_Table")] // To avoid conflict with SQL reserved word
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public int ClientID { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        public int? PaymentID { get; set; }
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Processing, Shipped, Delivered
        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();
    }
}