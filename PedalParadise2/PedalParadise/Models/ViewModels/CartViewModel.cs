using System;
using System.ComponentModel.DataAnnotations;

namespace PedalParadise.Models.ViewModels
{
    public class CartViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/geo
