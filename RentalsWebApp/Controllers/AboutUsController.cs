using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly ISendMail _sendMail;

        public AboutUsController(ISendMail sendMail)
        {
            _sendMail = sendMail;
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
        public IActionResult ReportIncident()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReportIncident(SendMailViewModel sendmailVM)
        {
            if (!ModelState.IsValid) return View("Error");

            await _sendMail.SendMailAsync(sendmailVM);
            TempData["SuccessMessage"] = "Thank you for your message. Our Agent will be in touch soon!";
            return RedirectToAction("Index", "Apartments");
        }
    }
}
