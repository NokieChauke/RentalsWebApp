using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public BankAccountController(IHttpContextAccessor httpContextAccessor, IBankAccountRepository bankAccountRepository
            , IDashboardRepository dashboardRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _bankAccountRepository = bankAccountRepository;
            _dashboardRepository = dashboardRepository;
        }
        [HttpGet]
        public async Task<IActionResult> AddBankAccount(string userId)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
                var addBankAccountVM = new BankingAccountViewModel { UserId = currentUserId };
                return View(addBankAccountVM);

            }
            else
            {
                var addBankAccountVM = new BankingAccountViewModel { UserId = userId };
                return View(addBankAccountVM);

            }

        }

        [HttpPost]
        public async Task<IActionResult> AddBankAccount(BankingAccountViewModel bankingAccountVM)
        {
            var accounts = await _bankAccountRepository.GetAllBankAccounts();

            foreach (var account in accounts)
            {
                account.Active = false;
                _bankAccountRepository.UpdateAccount(account);
            }
            if (ModelState.IsValid)
            {
                var bankingAccount = new BankAccount()
                {
                    AppUserId = bankingAccountVM.UserId,
                    CardDescreption = bankingAccountVM.CardDescreption,
                    BankName = bankingAccountVM.BankName,
                    AccountHolder = bankingAccountVM.AccountHolder,
                    CardNumber = bankingAccountVM.CardNumber,
                    BranchCode = bankingAccountVM.BranchCode,
                    ExpiryDate = bankingAccountVM.ExpiryDate,
                    CSV = bankingAccountVM.CSV
                };
                _bankAccountRepository.Add(bankingAccount);
                return RedirectToAction("Index", "Billing", new { id = bankingAccount.AppUserId });
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(bankingAccountVM);

        }
        [HttpGet]

        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id);
            if (account == null) return View("Error");
            var editBankingAccountVM = new EditBankingAccountViewModel
            {
                UserId = account.AppUserId,
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

            var account = await _bankAccountRepository.GetByIdAsyncNoTracking(id);

            if (account != null)
            {
                var bankAccount = new BankAccount
                {
                    Id = id,
                    AppUserId = editBankingAccountVM.UserId,
                    CardDescreption = editBankingAccountVM.CardDescreption,
                    BankName = editBankingAccountVM.BankName,
                    AccountHolder = editBankingAccountVM.AccountHolder,
                    CardNumber = editBankingAccountVM.CardNumber,
                    BranchCode = editBankingAccountVM.BranchCode,
                    ExpiryDate = editBankingAccountVM.ExpiryDate,
                    CSV = editBankingAccountVM.CSV,
                    Active = account.Active
                };

                _bankAccountRepository.UpdateAccount(bankAccount);
                return RedirectToAction("Index", "Billing", new { id = editBankingAccountVM.UserId });
            }
            else
            {
                return View(editBankingAccountVM);
            }
        }

        public async Task<IActionResult> SetAccountAsDefault(int id)
        {
            var accounts = await _bankAccountRepository.GetAllBankAccounts();

            foreach (var account in accounts)
            {
                account.Active = false;
                _bankAccountRepository.UpdateAccount(account);
            }

            var acc = await _bankAccountRepository.GetByIdAsync(id);
            acc.Active = true;
            _bankAccountRepository.UpdateAccount(acc);
            return RedirectToAction("Index", "Billing", new { id = acc.AppUserId });
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id);

            _bankAccountRepository.DeleteAccount(account);
            return RedirectToAction("Index", "Billing", new { id = account.AppUserId });
        }

    }
}
