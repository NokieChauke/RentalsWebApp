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
        public async Task<IActionResult> Index()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);
            if (docs != null)
            {
                var documentVM = new DocumentsDisplayViewModel()
                {
                    UserId = currentUserId,
                    IdCopy = Path.GetFileName(docs.IdCard),
                    Contract = Path.GetFileName(docs.Contract),
                    PaySlip = Path.GetFileName(docs.PaySlip)
                };
                return View(documentVM);

            }
            else
            {
                return RedirectToAction("NoDocuments");
            }
            return View();

        }
        public IActionResult NoDocuments()
        {
            return View();
        }

        public async Task<IActionResult> OpenIDCopy()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileName(docs.IdCard);
            string path = Path.Combine(webRootPath + "/documents/idcopy/" + fileName);

            return new FileStreamResult(new FileStream(path, FileMode.Open), "application/pdf");

        }
        public async Task<IActionResult> OpenContract()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileName(docs.Contract);
            string path = Path.Combine(webRootPath + "/documents/contract/" + fileName);

            return new FileStreamResult(new FileStream(path, FileMode.Open), "application/pdf");

        }
        public async Task<IActionResult> OpenPayslip()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileName(docs.PaySlip);
            string path = Path.Combine(webRootPath + "/documents/payslip/" + fileName);

            return new FileStreamResult(new FileStream(path, FileMode.Open), "application/pdf");

        }

        [HttpGet]
        public async Task<IActionResult> EditId(int id)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            var editDocumentsVM = new EditDocumentsViewModel
            {
                Id = id,
                AppUserId = currentUserId,

            };
            return View(editDocumentsVM);

        }
        [HttpPost]
        public async Task<IActionResult> EditId(int id, EditDocumentsViewModel editDocumentsVM)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            string iFileName = Path.GetFileNameWithoutExtension(editDocumentsVM.IdCopy.FileName);
            string iExtention = Path.GetExtension(editDocumentsVM.IdCopy.FileName);
            string idCopyFileName = iFileName + DateTime.Now.ToString("yymmddhhmm") + iExtention;
            string iPath = Path.Combine(webRootPath + "/documents/idcopy/", idCopyFileName);

            using (var fileStream = new FileStream(iPath, FileMode.Create))
            {
                await editDocumentsVM.IdCopy.CopyToAsync(fileStream);
            }

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);
            var oldFilePath = Path.Combine(webRootPath + "/documents/idcopy/", Path.GetFileName(docs.IdCard));

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
                ViewBag.deleteSuccess = "true";
            }
            docs.IdCard = iPath;

            _documentsRepository.Update(docs);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> EditContract(int id)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            var editDocumentsVM = new EditDocumentsViewModel
            {
                Id = id,
                AppUserId = currentUserId,

            };
            return View(editDocumentsVM);

        }
        [HttpPost]
        public async Task<IActionResult> EditContract(int id, EditDocumentsViewModel editDocumentsVM)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(editDocumentsVM.Contract.FileName);
            string extention = Path.GetExtension(editDocumentsVM.Contract.FileName);
            string idCopyFileName = fileName + DateTime.Now.ToString("yymmddhhmm") + extention;
            string path = Path.Combine(webRootPath + "/documents/contract/", idCopyFileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await editDocumentsVM.Contract.CopyToAsync(fileStream);
            }

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);
            var oldFilePath = Path.Combine(webRootPath + "/documents/contract/", Path.GetFileName(docs.Contract));

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
                ViewBag.deleteSuccess = "true";
            }
            docs.Contract = path;

            _documentsRepository.Update(docs);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> EditPayslip(int id)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            var editDocumentsVM = new EditDocumentsViewModel
            {
                Id = id,
                AppUserId = currentUserId,

            };
            return View(editDocumentsVM);

        }
        [HttpPost]
        public async Task<IActionResult> EditPayslip(int id, EditDocumentsViewModel editDocumentsVM)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            string iFileName = Path.GetFileNameWithoutExtension(editDocumentsVM.PaySlip.FileName);
            string iExtention = Path.GetExtension(editDocumentsVM.PaySlip.FileName);
            string idCopyFileName = iFileName + DateTime.Now.ToString("yymmddhhmm") + iExtention;
            string iPath = Path.Combine(webRootPath + "/documents/payslip/", idCopyFileName);

            using (var fileStream = new FileStream(iPath, FileMode.Create))
            {
                await editDocumentsVM.PaySlip.CopyToAsync(fileStream);
            }

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);
            var oldFilePath = Path.Combine(webRootPath + "/documents/payslip/", Path.GetFileName(docs.PaySlip));

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
                ViewBag.deleteSuccess = "true";
            }
            docs.PaySlip = iPath;

            _documentsRepository.Update(docs);
            return RedirectToAction("Index");


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

                using (var fileStream = new FileStream(pPath, FileMode.Create))
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
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Documents upload failed");

            }
            return View(documentsVM);

        }

        [HttpGet]
        public async Task<IActionResult> EditDocuments()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var docs = await _documentsRepository.GetUploadedDocuments(currentUserId);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileName(docs.IdCard);
            string path = Path.Combine(webRootPath + "/documents/payslip/" + fileName);

            var file = File(System.IO.File.OpenRead(path), "image/jpeg", Path.GetFileName(path));

            var editDocumentsVM = new EditDocumentsViewModel
            {
                AppUserId = currentUserId,
                Contract = (IFormFile)file,
                IdCopy = (IFormFile)file,
                PaySlip = (IFormFile)file


            };
            return View(editDocumentsVM);
        }
        public async Task<IActionResult> DeleteDocuments()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var documents = await _documentsRepository.GetUploadedDocuments(currentUserId);
            string webRootPath = _webHostEnvironment.WebRootPath;
            var oldIdCopyPath = Path.Combine(webRootPath + "/documents/idcopy/", Path.GetFileName(documents.IdCard));

            if (System.IO.File.Exists(oldIdCopyPath))
            {
                System.IO.File.Delete(oldIdCopyPath);
                ViewBag.deleteSuccess = "true";
            }
            var oldContractPath = Path.Combine(webRootPath + "/documents/contract/", Path.GetFileName(documents.Contract));

            if (System.IO.File.Exists(oldContractPath))
            {
                System.IO.File.Delete(oldContractPath);
                ViewBag.deleteSuccess = "true";
            }
            var oldFilePath = Path.Combine(webRootPath + "/documents/payslip/", Path.GetFileName(documents.PaySlip));

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
                ViewBag.deleteSuccess = "true";
            }
            if (documents == null) return View("Error");

            _documentsRepository.Delete(documents);
            return RedirectToAction("Index");
        }
    }

}
