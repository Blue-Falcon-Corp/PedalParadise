namespace PedalParadise.Models.ViewModels
{
    public class OrderPageViewModel
    {
        public ProfileViewModel Profile { get; set; } // Sidebar info
        public List<Order> Orders { get; set; }       // All orders
    }
}
