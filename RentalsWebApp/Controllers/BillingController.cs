﻿using Microsoft.AspNetCore.Mvc;
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

        public BillingController(IHttpContextAccessor httpContextAccessor, IBillingRepository billingRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _billingRepository = billingRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var month = await _billingRepository.GetMonth(currentUserId);
            var m = month.Month;
            Billing billing = await _billingRepository.GetMonthlyStatemets(currentUserId, m);
            IEnumerable<BankAccount> accounts = await _billingRepository.GetAll(currentUserId);

            var billingVM = new BillingViewModel()
            {
                Id = billing.Id,
                Month = billing.Month,
                WaterAmount = billing.WaterAmount,
                ElectricityAmount = billing.ElectricityAmount,
                BankAccount = (List<BankAccount>)accounts

            };
            return View(billingVM);

        }
        public IActionResult BankingDetails()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> UploadStatement(string id)
        {
            var user = await _billingRepository.GetUserById(id);
            var uploadStatement = new UploadStatementViewModel { UserId = user.Id };
            return View(uploadStatement);
        }
        [HttpPost]
        public async Task<IActionResult> UploadStatement(string id, UploadStatementViewModel uploadStatementVM)
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
                    Statement = statementUrl

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
        public async Task<IActionResult> DownloadStatement()
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
        [HttpGet]
        public async Task<IActionResult> AddBankAccount()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var addBankAccountVM = new BankingAccountViewModel { AppUserId = currentUserId };
            return View(addBankAccountVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddBankAccount(BankingAccountViewModel bankingAccountVM)
        {
            if (ModelState.IsValid)
            {
                var bankingAccount = new BankAccount()
                {
                    AppUserId = bankingAccountVM.AppUserId,
                    CardDescreption = bankingAccountVM.CardDescreption,
                    BankName = bankingAccountVM.BankName,
                    AccountHolder = bankingAccountVM.AccountHolder,
                    CardNumber = bankingAccountVM.CardNumber,
                    BranchCode = bankingAccountVM.BranchCode,
                    ExpiryDate = bankingAccountVM.ExpiryDate,
                    CSV = bankingAccountVM.CSV
                };
                _billingRepository.Add(bankingAccount);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(bankingAccountVM);

        }

        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _billingRepository.GetByIdAsync(id);
            if (account == null) return View("Error");
            var editBankingAccountVM = new EditBankingAccountViewModel
            {
                AppUserId = account.AppUserId,
                CardDescreption = account.CardDescreption,
                BankName = account.BankName,
                AccountHolder = account.AccountHolder,
                CardNumber = account.CardNumber,
                BranchCode = account.BranchCode,
                ExpiryDate = account.ExpiryDate,
                CSV = account.CSV

            };
            return View(editBankingAccountVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(int id, EditBankingAccountViewModel editBankingAccountVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit apartment");
                return View("Edit", editBankingAccountVM);
            }

            var account = await _billingRepository.GetByIdAsyncNoTracking(id);

            if (account != null)
            {
                var bankAccount = new BankAccount
                {
                    Id = id,
                    AppUserId = editBankingAccountVM.AppUserId,
                    CardDescreption = editBankingAccountVM.CardDescreption,
                    BankName = editBankingAccountVM.BankName,
                    AccountHolder = editBankingAccountVM.AccountHolder,
                    CardNumber = editBankingAccountVM.CardNumber,
                    BranchCode = editBankingAccountVM.BranchCode,
                    ExpiryDate = editBankingAccountVM.ExpiryDate,
                    CSV = editBankingAccountVM.CSV
                };

                _billingRepository.UpdateAccount(bankAccount);
                return RedirectToAction("Index");
            }
            else
            {
                return View(editBankingAccountVM);
            }
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
