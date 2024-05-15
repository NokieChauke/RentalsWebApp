using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IApartmentsRepository _apartmentsRepository;

        public DashboardController(IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService,
            UserManager<AppUser> userManager, IApartmentsRepository apartmentsRepository)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
            _userManager = userManager;
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
            user.ProfileImage = photoResult.Url.ToString();
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

        public async Task<IActionResult> EditUserProfile(string id)
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                var tenant = await _apartmentsRepository.GetUserById(id);
                if (tenant == null) return View("Error");
                var ediUserVM = new EditUserProfileViewModel()
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    Surname = tenant.Surname,
                    Email = tenant.Email,
                    PhoneNumber = tenant.PhoneNumber,
                    IdentityNo = tenant.IdentityNo,
                    URL = tenant.ProfileImage
                };
                return View(ediUserVM);
            }
            else
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
                    URL = user.ProfileImage
                };
                return View(ediUserVM);
            }

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
                URL = user.ProfileImage

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
                if (user.ProfileImage != null)
                {
                    try
                    {
                        await _photoService.DeletePhotonsAsync(user.ProfileImage);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete photo");
                        return View(ediUserVM);
                    }
                }

                var photoResult = await _photoService.AddProfilePhoto(ediUserVM.ProfileImageUrl);
                MapUserEdit(user, ediUserVM, photoResult);

                _dashboardRepository.UpdateUser(user);
                return RedirectToAction("Upload", "Documents");
            }
            else
            {
                return View(ediUserVM);
            }

        }
        public async Task<IActionResult> UserDetails(string id)
        {

            var apartment = await _dashboardRepository.GetApartmentByUserId(id);
            var user = await _dashboardRepository.GetUserById(id);
            if (user == null) return View("Error");
            var userViewModel = new UserProfileViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImage,
                Apartments = apartment
            };
            return View(userViewModel);

        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var apartment = await _dashboardRepository.GetApartmentByUserId(currentUserId);


            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                var tenantUser = await _apartmentsRepository.GetUserById(id);
                var tenantApartment = await _dashboardRepository.GetApartmentByUserId(tenantUser.Id);
                var tenent = await _dashboardRepository.GetCurrentUserById(tenantUser.Id);
                if (tenent == null) return View("Error");
                var tenantViewModel = new UserProfileViewModel()
                {
                    Id = tenent.Id,
                    Name = tenent.Name,
                    Surname = tenent.Surname,
                    PhoneNumber = tenent.PhoneNumber,
                    Email = tenent.Email,
                    ProfileImageUrl = tenent.ProfileImage,
                    Apartments = tenantApartment
                };
                return View(tenantViewModel);
            }

            var user = await _dashboardRepository.GetCurrentUserById(currentUserId);
            if (user == null) return View("Error");
            var userViewModel = new UserProfileViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImage,
                Apartments = apartment
            };
            return View(userViewModel);

        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userDetails = await _dashboardRepository.GetUserById(id);
            if (userDetails == null) return View("Error");

            _dashboardRepository.DeleteUser(userDetails);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Security()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(currentUserId);
            if (user == null) return View("Error");
            var secutityVM = new SecurityViewModel()
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber
            };

            return View(secutityVM);

        }
        [HttpPost]
        public async Task<IActionResult> Security(SecurityViewModel securityVM)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(currentUserId);
            var passwordCheck = await _userManager.CheckPasswordAsync(user, securityVM.CurrentPassword);
            if (ModelState.IsValid)
            {
                if (passwordCheck)
                {
                    var result = await _userManager.ChangePasswordAsync(user, securityVM.CurrentPassword, securityVM.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserProfile", "Dashboard");
                    }
                    var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                    ModelState.AddModelError("", message);
                    return View(securityVM);
                }

            }

            TempData["Error"] = "Wrong password. Please try again";
            return View(securityVM);

        }

        [HttpGet]
        public async Task<IActionResult> Notification()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var notification = await _dashboardRepository.GetNotificationsByUserId(currentUserId);

            if (notification == null)
            {
                var notificationsVM = new NotificationsViewModel()
                {
                    UserId = currentUserId,
                    SMS = true,
                    Email = true,
                    AccountChanges = true,
                    StatementUpload = true,
                    NewApartment = true,
                    TermsNConditions = true,
                    RentIncrease = true,
                    Security = true
                };
                return View(notificationsVM);
            }
            else
            {
                var notificationsVM = new NotificationsViewModel()
                {
                    Id = notification.Id,
                    UserId = notification.UserId,
                    PhoneNumber = notification.AppUser.PhoneNumber,
                    EmailAddress = notification.AppUser.Email,
                    SMS = notification.SMS,
                    Email = notification.Email,
                    AccountChanges = notification.AccountChanges,
                    StatementUpload = notification.StatementUpload,
                    NewApartment = notification.NewApartment,
                    TermsNConditions = notification.TermsNConditions,
                    RentIncrease = notification.RentIncrease,
                    Security = notification.Security
                };
                return View(notificationsVM);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Notification(NotificationsViewModel notificationsVM)
        {
            if (ModelState == null) return View("Error");

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userNotification = _dashboardRepository.GetNotificationByUserIdNoTracking(currentUserId);


            if (userNotification != null)
            {
                var notifications = new Notifications
                {
                    Id = notificationsVM.Id,
                    UserId = notificationsVM.UserId,
                    SMS = notificationsVM.SMS,
                    Email = notificationsVM.Email,
                    AccountChanges = notificationsVM.AccountChanges,
                    StatementUpload = notificationsVM.StatementUpload,
                    NewApartment = notificationsVM.NewApartment,
                    TermsNConditions = notificationsVM.TermsNConditions,
                    RentIncrease = notificationsVM.RentIncrease,
                    Security = notificationsVM.Security
                };
                _dashboardRepository.UpdateNotifications(notifications);
                return RedirectToAction("EditUserProfile");
            }
            else
            {
                var notifications = new Notifications
                {
                    UserId = notificationsVM.UserId,
                    SMS = notificationsVM.SMS,
                    Email = notificationsVM.Email,
                    AccountChanges = notificationsVM.AccountChanges,
                    StatementUpload = notificationsVM.StatementUpload,
                    NewApartment = notificationsVM.NewApartment,
                    TermsNConditions = notificationsVM.TermsNConditions,
                    RentIncrease = notificationsVM.RentIncrease,
                    Security = notificationsVM.Security
                };
                _dashboardRepository.AddUserNotifications(notifications);
                return RedirectToAction("EditUserProfile");
            }

        }
    }
}
