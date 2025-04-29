using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models.ViewModels;
using PedalParadise.Services;

namespace PedalParadise.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRepairService _repairService;
        private readonly IRefundService _refundService;
        private readonly IUserService _userService;

        public RequestController(IRepairService repairService, IRefundService refundService, IUserService userService)
        {
            _repairService = repairService;
            _refundService = refundService;
            _userService = userService;
        }

        [Route("/Request/List")]
        public async Task<IActionResult> List()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserByIdAsync(userId.Value);

            var profileModel = new ProfileViewModel { User = user };
            var repairRequests = await _repairService.GetAllRepairsAsync();
            var refundRequests = await _refundService.GetAllRefundsAsync();

            var viewModel = new RequestPageViewModel
            {
                Profile = profileModel,
                RepairRequests = (List<Models.RepairRequest>)repairRequests,
                RefundRequests = (List<Models.RefundRequest>)refundRequests
            };

            return View(viewModel);
        }
    }
}
