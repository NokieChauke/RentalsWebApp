using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDBContext _context;
        private readonly IApartmentsRepository _apartmentsRepository;
        private readonly ISendMail _sendMail;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ApplicationDBContext context, IApartmentsRepository apartmentsRepository, ISendMail sendMail)
        {
            _context = context;
            _apartmentsRepository = apartmentsRepository;
            _sendMail = sendMail;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
                        {
                            return RedirectToAction("UserProfile", "Dashboard");

                        }

                    }


                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginVM);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginVM);


        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var response = new ForgotPasswordViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (!ModelState.IsValid) return View(forgotPasswordVM);

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.EmailAddress);

            if (user != null)
            {
                await _sendMail.ResetPassword(forgotPasswordVM);
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Error"] = "User does not exist";
                return View(forgotPasswordVM);
            }
        }
        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            var response = new LoginViewModel();
            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> Register(int Id)
        {
            //Apartments apartments = await _apartmentsRepository.GetByIdAsync(Id);
            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerVM)
        {

            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is alredy in use";
                return View(registerVM);
            }
            var newUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                IdentityNo = registerVM.IdentityNo,
                PhoneNumber = registerVM.PhoneNumber,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Tenant);
                await _sendMail.NewUserEmail(registerVM);
                return RedirectToAction("Index", "Dashboard");
            }

            var message = string.Join(", ", newUserResponse.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
            ModelState.AddModelError("", message);

            return View(registerVM);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Apartments");
        }
    }
}
