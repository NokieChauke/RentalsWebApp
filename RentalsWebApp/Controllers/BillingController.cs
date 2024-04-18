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


        public BillingController(IHttpContextAccessor httpContextAccessor, IBillingRepository billingRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _billingRepository = billingRepository;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Index(string id)
        {
            var month = await _billingRepository.GetMonth(id);
            var m = month.Month;
            Billing billing = await _billingRepository.GetMonthlyStatemets(id, m);
            IEnumerable<BankAccount> accounts = await _billingRepository.GetAll(id);

            var billingVM = new BillingViewModel()
            {
                Id = billing.Id,
                Month = billing.Month,
                WaterAmount = billing.WaterAmount,
                ElectricityAmount = billing.ElectricityAmount,
                BankAccount = (List<BankAccount>)accounts,
                UserId = billing.UserId,

            };
            return View(billingVM);

        }
        public IActionResult BankingDetails()
        {
            return View();

        }
        public async Task<IActionResult> PaymentHistory(string id)
        {
            var user = await _billingRepository.GetUserById(id);
            var billind = new BillingViewModel { UserId = user.Id };
            return View(billind);


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
            var accounts = await _billingRepository.GetAllBankAccounts();

            foreach (var account in accounts)
            {
                account.Active = false;
                _billingRepository.UpdateAccount(account);
            }
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
                return RedirectToAction("Index", new { id = bankingAccount.AppUserId });
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(bankingAccountVM);

        }
        [HttpGet]
        public async Task<IActionResult> AddBankAccountByAdmin(string id)
        {
            var user = await _billingRepository.GetUserById(id);

            var addBankAccountVM = new BankingAccountViewModel { AppUserId = user.Id };
            return View(addBankAccountVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddBankAccountByAdmin(BankingAccountViewModel bankingAccountVM)
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
                return RedirectToAction("Index", new { id = bankingAccount.AppUserId });
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

        public async Task<IActionResult> SetAccountAsDefault(int id)
        {
            var accounts = await _billingRepository.GetAllBankAccounts();

            foreach (var account in accounts)
            {
                account.Active = false;
                _billingRepository.UpdateAccount(account);
            }

            var acc = await _billingRepository.GetByIdAsync(id);
            acc.Active = true;
            _billingRepository.UpdateAccount(acc);
            return RedirectToAction("Index", new { id = acc.AppUserId });
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _billingRepository.GetByIdAsync(id);

            _billingRepository.DeleteAccount(account);
            return RedirectToAction("Index", new { id = account.AppUserId });
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
                return RedirectToAction("Index", new { id = editBankingAccountVM.AppUserId });
            }
            else
            {
                return View(editBankingAccountVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UploadProofOfPayment(string id)
        {
            var user = await _billingRepository.GetUserById(id);
            var uploadPOP = new ProofOfPaymentViewModel { UserId = user.Id };
            return View(uploadPOP);
        }
        [HttpPost]
        public async Task<IActionResult> UploadProofOfPayment(string id, ProofOfPaymentViewModel proofOfPaymentVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(proofOfPaymentVM.Proof.FileName);
                string extention = Path.GetExtension(proofOfPaymentVM.Proof.FileName);
                string proofName = fileName + DateTime.Now.ToString("yymmddhhmm") + extention;
                string proofUrl = Path.Combine(webRootPath + "/documents/pop/", proofName);

                using (var fileStream = new FileStream(proofUrl, FileMode.Create))
                {
                    await proofOfPaymentVM.Proof.CopyToAsync(fileStream);
                }
                var proofOfPayment = new ProofOfPayment()
                {
                    UserId = proofOfPaymentVM.UserId,
                    Month = proofOfPaymentVM.Month,
                    Proof = proofUrl,


                };
                _billingRepository.UploadProofOfPayment(proofOfPayment);
                return RedirectToAction("Index", new { id = proofOfPaymentVM.UserId });
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(proofOfPaymentVM);

        }
        public async Task<IActionResult> DownloadProofOfpayment()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var proof = _billingRepository.DownloadProofOfPayment(currentUserId);

            string path = proof.Result.Proof;

            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return NotFound();


        }
    }

}
