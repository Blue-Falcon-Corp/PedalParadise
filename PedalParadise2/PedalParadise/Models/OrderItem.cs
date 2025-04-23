using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PedalParadise.Models
{
    [Keyless]
    public class OrderItem
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Navigation properties
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
