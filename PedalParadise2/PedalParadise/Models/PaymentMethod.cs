using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PedalParadise.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
