<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PedalParadise.Models
{
    [Keyless]
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedalParadise.Models
{
>>>>>>> origin/geo
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
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/geo
