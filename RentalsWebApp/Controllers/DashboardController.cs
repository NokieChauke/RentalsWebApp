using CloudinaryDotNet.Actions;
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
        private void MapUserEdit(AppUser user, EditUserProfileViewModel editUserVM, ImageUploadResult photoResult)
        {
            user.Id = editUserVM.Id;
            user.Name = editUserVM.Name;
            user.Surname = editUserVM.Surname;
            user.Email = editUserVM.Email;
            user.PhoneNumber = editUserVM.PhoneNumber;
            user.IdentityNo = editUserVM.IdentityNo;
            user.ProfileImageUrl = photoResult.Url.ToString();
        }
        public async Task<IActionResult> Index()
        {
            var user = new AppUser();
            var tenents = await _dashboardRepository.GetAllTenants(user);
            List<TenantsListViewModel> result = new List<TenantsListViewModel>();
            foreach (var tenant in tenents)
            {
                var tenantsListViewModel = new TenantsListViewModel()
                {
                    Id = tenant.Id,
                    PhoneNumber = tenant.PhoneNumber,
                    Name = tenant.Name,
                    Surname = tenant.Surname
                };
                result.Add(tenantsListViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(currentUserId);
            if (user == null) return View("Error");
            var ediUserVM = new EditUserProfileViewModel()
            {
                Id = currentUserId,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentityNo = user.IdentityNo,
                URL = user.ProfileImageUrl
            };
            return View(ediUserVM);
        }
        public async Task<IActionResult> EditUserProfileByAdmin(string id)
        {
            var user = await _dashboardRepository.GetUserById(id);
            if (user == null) return View("Error");
            var userViewModel = new EditUserProfileViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentityNo = user.IdentityNo,
                URL = user.ProfileImageUrl

            };
            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel ediUserVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit the User Profile");
                return View("EditUserProfile", ediUserVM);
            }

            var user = await _dashboardRepository.GetUserByIdNoTracking(ediUserVM.Id);

            if (user != null)
            {
                if (user.ProfileImageUrl != null)
                {
                    try
                    {
                        await _photoService.DeletePhotonsAsync(user.ProfileImageUrl);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete photo");
                        return View(ediUserVM);
                    }
                }

                var photoResult = await _photoService.AddPhotoAsync(ediUserVM.ProfileImageUrl);
                MapUserEdit(user, ediUserVM, photoResult);

                _dashboardRepository.UpdateUser(user);
                return RedirectToAction("Index", "Documents");
            }
            else
            {
                return View(ediUserVM);
            }

        }

        public async Task<IActionResult> UserProfile(string id)
        {
            IEnumerable<Apartments> apartments = await _apartmentsRepository.GetAll();
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _dashboardRepository.GetCurrentUserById(currentUserId);
            if (user == null) return View("Error");
            var userViewModel = new UserProfileViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                Apartments = (List<Apartments>)apartments
            };
            return View(userViewModel);

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
