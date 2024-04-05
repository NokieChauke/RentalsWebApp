using Microsoft.AspNetCore.Mvc;

namespace RentalsWebApp.Controllers
{
    public class UserLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
