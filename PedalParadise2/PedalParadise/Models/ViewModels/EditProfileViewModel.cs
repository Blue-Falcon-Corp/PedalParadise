using System.ComponentModel.DataAnnotations;
namespace PedalParadise.Models.ViewModels
{
    public class EditProfileViewModel
    {
        public int UserID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [EmailAddress]
        [System.ComponentModel.ReadOnly(true)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Address")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
    }
}