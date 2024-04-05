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

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ApplicationDBContext context, IApartmentsRepository apartmentsRepository)
        {
            _context = context;
            _apartmentsRepository = apartmentsRepository;
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
        public IActionResult Rent()
        {
            var response = new LoginViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Rent(LoginViewModel loginVM)
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
                await _userManager.AddToRoleAsync(newUser, UserRoles.Tenant);

            return View("Login");
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Apartments");
        }
    }
}
