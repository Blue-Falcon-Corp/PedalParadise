using System.ComponentModel.DataAnnotations;

namespace PedalParadise.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Name on Card")]
        public string CardName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{2}/\d{2}$", ErrorMessage = "Expiry must be in MM/YY format")]
        [Display(Name = "Card Expiration Date")]
        public string CardExpiry { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        [Display(Name = "Security Code")]
        public string CardCVV { get; set; } = string.Empty;
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/geo
