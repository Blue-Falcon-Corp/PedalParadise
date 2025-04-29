namespace PedalParadise.Models.ViewModels
{
    public class RequestPageViewModel
    {
        public ProfileViewModel Profile { get; set; } // Sidebar info
        public List<RepairRequest> RepairRequests { get; set; } // All repair requests
        public List<RefundRequest> RefundRequests { get; set; } // All refund requests
    }
}
