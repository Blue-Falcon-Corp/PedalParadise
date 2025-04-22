namespace PedalParadise.Models
{
    public class Client : User
    {
        public int ClientID { get; set; }
        public string MembershipType { get; set; } // Basic, Premium, etc.
    }
}
