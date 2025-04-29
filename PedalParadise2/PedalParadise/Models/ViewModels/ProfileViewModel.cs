namespace PedalParadise.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Order> Orders { get; set; }

        public Client? Client => User as Client;

        public Employee Employee => User as Employee;   
    }
}
