using System.ComponentModel.DataAnnotations;
namespace PedalParadise.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [StringLength(20)]
        public string UserType { get; set; } = string.Empty; // Employee, Client

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual Client? Client { get; set; }
    }
}