namespace PedalParadise.Models.ViewModels
{
    public class AccountPageViewModel
    {
        public ProfileViewModel Profile { get; set; } // Sidebar info
        public List<Client> Clients { get; set; }     // List of Clients
        public List<Employee> Employees { get; set; } // List of Employees
    }
}
