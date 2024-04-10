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

        public BillingController(IHttpContextAccessor httpContextAccessor, IBillingRepository billingRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _billingRepository = billingRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> StatementUpload()
        {

            return View();
        }

        [HttpGet]
        public IActionResult UploadStatement()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadStatement(UploadStatementViewModel uploadStatementVM)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (ModelState.IsValid)
            {
                var billing = new Billing()
                {
                    UserId = currentUserId,
                    Month = uploadStatementVM.Month,
                    WaterAmount = uploadStatementVM.WaterAmount,
                    ElectricityAmount = uploadStatementVM.ElectricityAmount,
                    Statement = uploadStatementVM.StatementUrl,

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

        [HttpPost]
        public async Task<IActionResult> AddBankAccount(BankingAccountViewModel bankingAccountVM)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (ModelState.IsValid)
            {
                var bankingAccount = new BankAccount()
                {
                    AppUserId = currentUserId,
                    CardDescreption = bankingAccountVM.CardDescreption,
                    BankName = bankingAccountVM.BankName,
                    AccountHolder = bankingAccountVM.AccountHolder,
                    CardNumber = bankingAccountVM.CardNumber,
                    BranchCode = bankingAccountVM.BranchCode,
                    ExpiryDate = bankingAccountVM.ExpiryDate,
                    CSV = bankingAccountVM.CSV
                };
                _billingRepository.Add(bankingAccount);
                return RedirectToAction("Billing");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(bankingAccountVM);

        }
        [HttpGet]
        public IActionResult UploadProofOfPayment()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadProofOfPayment(ProofOfPaymentViewModel proofOfPaymentVM)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (ModelState.IsValid)
            {
                var proofOfPayment = new ProofOfPayment()
                {
                    UserId = currentUserId,
                    Month = proofOfPaymentVM.Month,
                    Proof = proofOfPaymentVM.ProofUrl,


                };
                _billingRepository.UploadProofOfPayment(proofOfPayment);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(proofOfPaymentVM);

        }
    }
}
