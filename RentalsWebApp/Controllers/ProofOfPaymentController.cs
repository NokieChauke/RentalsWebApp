using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class ProofOfPaymentController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProofOfPaymentRepository _proofOfPaymentRepository;
        private readonly IBillingRepository _billingRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProofOfPaymentController(IHttpContextAccessor httpContextAccessor, IProofOfPaymentRepository proofOfPaymentRepository
            , IBillingRepository billingRepository, IDashboardRepository dashboardRepository, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _proofOfPaymentRepository = proofOfPaymentRepository;
            _billingRepository = billingRepository;
            _dashboardRepository = dashboardRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> UploadProofOfPayment(string id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                var user = await _dashboardRepository.GetUserById(currentUserId);

                var uploadPOP = new ProofOfPaymentViewModel
                {
                    UserId = user.Id,
                };
                return View(uploadPOP);
            }
            else
            {
                var user = await _dashboardRepository.GetUserById(id);

                var uploadPOP = new ProofOfPaymentViewModel
                {
                    UserId = user.Id,
                };
                return View(uploadPOP);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadProofOfPayment(ProofOfPaymentViewModel proofOfPaymentVM)
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
                if (User.Identity.IsAuthenticated && User.IsInRole("tenant"))
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                    var bill = await _billingRepository.GetStatementByUserId(currentUserId, proofOfPaymentVM.Month);
                    if (bill != null)
                    {
                        var proofOfPayment = new ProofOfPayment()
                        {

                            UserId = proofOfPaymentVM.UserId,
                            Month = proofOfPaymentVM.Month,
                            Proof = proofUrl,


                        };

                        _proofOfPaymentRepository.UploadProofOfPayment(proofOfPayment);
                        bill.ProofOfPaymentId = proofOfPayment.Id;
                        _billingRepository.Update(bill);
                        return RedirectToAction("Index", "Billing", new { id = proofOfPaymentVM.UserId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "There's no bill for the selected month");
                    }
                }
                else
                {
                    var bill = await _billingRepository.GetStatementByUserId(proofOfPaymentVM.UserId, proofOfPaymentVM.Month);
                    if (bill != null)
                    {
                        var proofOfPayment = new ProofOfPayment()
                        {

                            UserId = proofOfPaymentVM.UserId,
                            Month = proofOfPaymentVM.Month,
                            Proof = proofUrl,


                        };

                        _proofOfPaymentRepository.UploadProofOfPayment(proofOfPayment);
                        bill.ProofOfPaymentId = proofOfPayment.Id;
                        _billingRepository.Update(bill);
                        return RedirectToAction("Index", "Billing", new { id = proofOfPaymentVM.UserId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "There's no bill for the selected month");
                    }
                }


            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return View(proofOfPaymentVM);

        }
        public async Task<IActionResult> DownloadProofOfpayment(int id)
        {
            var proof = _proofOfPaymentRepository.DownloadProofOfPayment(id);

            string path = proof.Result.Proof;

            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return NotFound();


        }
    }
}
