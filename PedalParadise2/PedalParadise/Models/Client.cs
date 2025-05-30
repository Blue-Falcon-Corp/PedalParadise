﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class Client
    {
        [Key]
        [ForeignKey("User")]
        public int ClientID { get; set; }
        [StringLength(20)]
        public string Membership { get; set; } = string.Empty;
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
    }

}