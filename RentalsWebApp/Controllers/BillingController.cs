using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class BillingController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBillingRepository _billingRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApartmentsRepository _apartmentsRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IProofOfPaymentRepository _proofOfPaymentRepository;

        public BillingController(IHttpContextAccessor httpContextAccessor, IBillingRepository billingRepository,
            IWebHostEnvironment webHostEnvironment, IApartmentsRepository apartmentsRepository, IDashboardRepository dashboardRepository
            , IBankAccountRepository bankAccountRepository, IProofOfPaymentRepository proofOfPaymentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _billingRepository = billingRepository;
            _webHostEnvironment = webHostEnvironment;
            _apartmentsRepository = apartmentsRepository;
            _dashboardRepository = dashboardRepository;
            _bankAccountRepository = bankAccountRepository;
            _proofOfPaymentRepository = proofOfPaymentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                var bill = await _billingRepository.GetBillByUserId(Id);

                if (bill != null)
                {
                    var billMonth = bill.Month;
                    var apartment = await _apartmentsRepository.GetByUserId(Id);
                    if (apartment != null)
                    {
                        Billing billing = await _billingRepository.GetStatementByUserId(Id, billMonth);
                        IEnumerable<BankAccount> accounts = await _bankAccountRepository.GetAll(Id);

                        var billingVM = new BillingViewModel()
                        {
                            Id = billing.Id,
                            Month = billing.Month,
                            WaterAmount = billing.WaterAmount,
                            Rent = apartment.Price,
                            ElectricityAmount = billing.ElectricityAmount,
                            BankAccount = (List<BankAccount>)accounts,
                            UserId = billing.UserId,

                        };
                        return View(billingVM);

                    }
                    return View("NoApartment");

                }
                return View("NoBill");
            }
            else
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                var month = await _billingRepository.GetBillByUserId(currentUserId);
                if (month != null)
                {
                    var m = month.Month;
                    var apartment = await _apartmentsRepository.GetByUserId(currentUserId);
                    if (apartment != null)
                    {
                        Billing billing = await _billingRepository.GetStatementByUserId(currentUserId, m);
                        IEnumerable<BankAccount> accounts = await _bankAccountRepository.GetAll(currentUserId);

                        var billingVM = new BillingViewModel()
                        {
                            Id = billing.Id,
                            Month = billing.Month,
                            WaterAmount = billing.WaterAmount,
                            Rent = apartment.Price,
                            ElectricityAmount = billing.ElectricityAmount,
                            BankAccount = (List<BankAccount>)accounts,
                            UserId = billing.UserId,

                        };
                        return View(billingVM);

                    }
                    return View("NoApartment");

                }
                return View("NoBill");
            }


        }
        public async Task<IActionResult> BankingDetails(string id)
        {
            var user = await _dashboardRepository.GetUserById(id);
            var billind = new BillingViewModel { UserId = user.Id };
            return View(billind);

        }
        public async Task<IActionResult> PaymentHistory(string Id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                Apartments apartment = await _apartmentsRepository.GetByUserId(currentUserId);

                IEnumerable<Billing> billings = await _billingRepository.GetAllBillingsByUserId(currentUserId);
                var billingVM = new PaymentHistoryViewModel()
                {
                    Billing = (List<Billing>)billings,
                    Rent = apartment.Price,

                };
                return View(billingVM);
            }
            else
            {
                Apartments apartment = await _apartmentsRepository.GetByUserId(Id);

                IEnumerable<Billing> billings = await _billingRepository.GetAllBillingsByUserId(Id);
                var billingVM = new PaymentHistoryViewModel()
                {
                    Billing = (List<Billing>)billings,
                    Rent = apartment.Price,

                };
                return View(billingVM);

            }

        }

        [HttpGet]
        public async Task<IActionResult> UploadStatement(string id)
        {
            var user = await _dashboardRepository.GetUserById(id);

            Random generator = new Random();
            string r = generator.Next(0, 1000000).ToString("D6");

            var uploadStatement = new UploadStatementViewModel
            {
                UserId = user.Id,
                TransactionId = string.Format("{0}{1}", "#", r)
            };
            return View(uploadStatement);
        }
        [HttpPost]
        public async Task<IActionResult> UploadStatement(UploadStatementViewModel uploadStatementVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(uploadStatementVM.Statement.FileName);
                string extention = Path.GetExtension(uploadStatementVM.Statement.FileName);
                string statementName = fileName + DateTime.Now.ToString("yymmddhhmm") + extention;
                string statementUrl = Path.Combine(webRootPath + "/documents/statements/", statementName);

                using (var fileStream = new FileStream(statementUrl, FileMode.Create))
                {
                    await uploadStatementVM.Statement.CopyToAsync(fileStream);
                }
                var billing = new Billing()
                {
                    UserId = uploadStatementVM.UserId,
                    Month = uploadStatementVM.Month,
                    WaterAmount = uploadStatementVM.WaterAmount,
                    ElectricityAmount = uploadStatementVM.ElectricityAmount,
                    Statement = statementUrl,
                    PaymentStatus = uploadStatementVM.Status,
                    TransactionId = uploadStatementVM.TransactionId
                };
                _billingRepository.UploadStatement(billing);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(uploadStatementVM);

        }
        public async Task<IActionResult> DownloadStatement(string Id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                var statement = _billingRepository.DownloadStatement(currentUserId);

                string path = statement.Result.Statement;

                if (System.IO.File.Exists(path))
                {
                    return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
                }
                return NotFound();
            }
            else
            {
                var statement = _billingRepository.DownloadStatement(Id);

                string path = statement.Result.Statement;

                if (System.IO.File.Exists(path))
                {
                    return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
                }
                return NotFound();
            }
        }
        public IActionResult NoBill()
        {
            return View();
        }


    }

}
