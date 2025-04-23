using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        [Required]
        public int ClientID { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/geo
