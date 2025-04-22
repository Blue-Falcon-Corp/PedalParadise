using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedalParadise.Models
{
    public class Client : User
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientID { get; }
        public string MembershipType { get; set; } // Basic, Premium, etc.
    }
}
