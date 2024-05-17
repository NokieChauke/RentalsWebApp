using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly ISendMail _sendMail;
        private readonly IDashboardRepository _dashboardRepository;

        public AboutUsController(ISendMail sendMail, IDashboardRepository dashboardRepository)
        {
            _sendMail = sendMail;
            _dashboardRepository = dashboardRepository;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(SendMailViewModel sendmailVM)
        {
            if (!ModelState.IsValid) return View("Error");

            await _sendMail.SendMailAsync(sendmailVM);
            TempData["SuccessMessage"] = "Thank you for your message. Our Agent will be in touch soon!";
            return RedirectToAction("Index", "Apartments");
        }
        [HttpGet]
        public async Task<IActionResult> ReportIncident(string UserId)
        {
            var user = _dashboardRepository.GetUserById(UserId);
            var apartment = _dashboardRepository.GetApartmentByUserId(UserId);
            if (user == null) return View("Error");

            var reportIncidentVM = new ReportIncidentViewModel
            {
                UserId = UserId,
                ApartmentId = apartment.Id,
            };

            return View(reportIncidentVM);
        }

        [HttpPost]
        public async Task<IActionResult> ReportIncident(ReportIncidentViewModel reportIncidentVM)
        {
            if (!ModelState.IsValid) return View("Error");

            await _sendMail.ReportIncident(reportIncidentVM);
            TempData["SuccessMessage"] = "Thank you for your message. Our Agent will be in touch soon!";
            return RedirectToAction("Index", "Apartments");
        }
    }
}
