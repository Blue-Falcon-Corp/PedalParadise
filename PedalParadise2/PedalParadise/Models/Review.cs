using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PedalParadise.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int ClientID { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual Client? Client { get; set; }
    }
}