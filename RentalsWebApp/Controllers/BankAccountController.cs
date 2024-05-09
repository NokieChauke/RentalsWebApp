﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddBankAccount()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var addBankAccountVM = new BankingAccountViewModel { AppUserId = currentUserId };
            return View(addBankAccountVM);
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
                    AppUserId = bankingAccountVM.AppUserId,
                    CardDescreption = bankingAccountVM.CardDescreption,
                    BankName = bankingAccountVM.BankName,
                    AccountHolder = bankingAccountVM.AccountHolder,
                    CardNumber = bankingAccountVM.CardNumber,
                    BranchCode = bankingAccountVM.BranchCode,
                    ExpiryDate = bankingAccountVM.ExpiryDate,
                    CSV = bankingAccountVM.CSV
                };
                _bankAccountRepository.Add(bankingAccount);
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
            var user = await _dashboardRepository.GetUserById(id);

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
                _bankAccountRepository.Add(bankingAccount);
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
            var account = await _bankAccountRepository.GetByIdAsync(id);
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

            var account = await _bankAccountRepository.GetByIdAsyncNoTracking(id);

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
                    CSV = editBankingAccountVM.CSV,
                    Active = account.Active
                };

                _bankAccountRepository.UpdateAccount(bankAccount);
                return RedirectToAction("Index", new { id = editBankingAccountVM.AppUserId });
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
            return RedirectToAction("Index", new { id = acc.AppUserId });
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id);

            _bankAccountRepository.DeleteAccount(account);
            return RedirectToAction("Index", new { id = account.AppUserId });
        }

    }
}