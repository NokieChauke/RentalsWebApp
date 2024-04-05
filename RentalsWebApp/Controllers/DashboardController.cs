using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;
        private readonly IApartmentsRepository _apartmentsRepository;

        public DashboardController(IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService,
            IApartmentsRepository apartmentsRepository)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
            _apartmentsRepository = apartmentsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = new AppUser();
            var tenents = await _dashboardRepository.GetAllTenants(user);
            List<DashboardViewModel> result = new List<DashboardViewModel>();
            foreach (var tenant in tenents)
            {
                var dashboardViewModel = new DashboardViewModel()
                {
                    Id = tenant.Id,
                    PhoneNumber = tenant.PhoneNumber,
                    Name = tenant.Name,
                    Surname = tenant.Surname
                };
                result.Add(dashboardViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _dashboardRepository.GetCurrentUserById(currentUserId);
            if (user == null) return View("Error");
            var userViewModel = new DashboardViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImage = user.ProfileImage,
                IdentityNo = user.IdentityNo,
                BankAccountId = user.BankAccountId,
                BankAccount = user.BankAccount,
                DocomentsId = user.DocomentsId,
                Documents = user.Documents

            };
            return View(userViewModel);
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            IEnumerable<Apartments> apartments = await _apartmentsRepository.GetAll();
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _dashboardRepository.GetCurrentUserById(currentUserId);
            if (user == null) return View("Error");
            var userViewModel = new DashboardViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Apartments = (List<Apartments>)apartments
            };
            return View(userViewModel);

        }
        public async Task<IActionResult> Documents()
        {

            return View();
        }
        public async Task<IActionResult> Billing()
        {

            return View();
        }
        public async Task<IActionResult> Security()
        {

            return View();
        }
        public async Task<IActionResult> Notification()
        {

            return View();
        }
    }
}
