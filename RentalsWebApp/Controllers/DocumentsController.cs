using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentsController(ApplicationDBContext applicationDBContext, IHttpContextAccessor httpContextAccessor
            , IDocumentsRepository documentsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDBContext = applicationDBContext;
            _httpContextAccessor = httpContextAccessor;
            _documentsRepository = documentsRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var docs = await _documentsRepository.GetUploadedDocuments(id);
            if (docs != null)
            {
                var documentVM = new DocumentsDisplayViewModel()
                {
                    UserId = id,
                    IdCopy = docs.IdCard,
                    Contract = docs.Contract,
                    PaySlip = docs.Contract
                };
                return View(documentVM);

            }
            return View();

        }
        [HttpGet]
        public IActionResult Upload()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var documentsViewModel = new DocumentsViewModel { AppUserId = currentUserId };
            return View(documentsViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(DocumentsViewModel documentsVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string iFileName = Path.GetFileNameWithoutExtension(documentsVM.IdCard.FileName);
                string iExtention = Path.GetExtension(documentsVM.IdCard.FileName);
                string idCopyFileName = iFileName + DateTime.Now.ToString("yymmddhhmm") + iExtention;
                string iPath = Path.Combine(webRootPath + "/documents/idcopy/", idCopyFileName);

                using (var fileStream = new FileStream(iPath, FileMode.Create))
                {
                    await documentsVM.IdCard.CopyToAsync(fileStream);
                }

                string cFileName = Path.GetFileNameWithoutExtension(documentsVM.Contract.FileName);
                string cExtention = Path.GetExtension(documentsVM.Contract.FileName);
                string contractFileName = cFileName + DateTime.Now.ToString("yymmddhhmm") + cExtention;
                string cPath = Path.Combine(webRootPath + "/documents/contract/", contractFileName);

                using (var fileStream = new FileStream(cPath, FileMode.Create))
                {
                    await documentsVM.Contract.CopyToAsync(fileStream);
                }

                string pFileName = Path.GetFileNameWithoutExtension(documentsVM.PaySlip.FileName);
                string pExtention = Path.GetExtension(documentsVM.PaySlip.FileName);
                string payslipFileName = pFileName + DateTime.Now.ToString("yymmddhhmm") + pExtention;
                string pPath = Path.Combine(webRootPath + "/documents/payslip/", payslipFileName);

                using (var fileStream = new FileStream(cPath, FileMode.Create))
                {
                    await documentsVM.PaySlip.CopyToAsync(fileStream);
                }

                var newDocuments = new Documents()
                {
                    AppUserId = documentsVM.AppUserId,
                    IdCard = iPath,
                    Contract = cPath,
                    PaySlip = pPath
                };

                _documentsRepository.Add(newDocuments);
                return RedirectToAction("Index", "Billing", new { id = newDocuments.AppUserId });

            }
            else
            {
                ModelState.AddModelError("", "Documents upload failed");

            }
            return View(documentsVM);

        }
    }

}
