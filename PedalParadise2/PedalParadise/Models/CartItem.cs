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
    public class CartItem
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual ShoppingCart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/geo
