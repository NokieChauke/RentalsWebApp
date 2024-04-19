using Microsoft.AspNetCore.Mvc;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;
using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly IApartmentsRepository _apartmentsRepository;
        private readonly IPhotoService _photoService;

        public ApartmentsController(IApartmentsRepository apartmentsRepository, IPhotoService photoService)
        {

            _apartmentsRepository = apartmentsRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Apartments> apartments = await _apartmentsRepository.GetAll();
            return View(apartments);
        }

        public async Task<IActionResult> Details(int id)
        {
            Apartments apartments = await _apartmentsRepository.GetByIdAsync(id);
            return View(apartments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateApartmentViewModel apartmentsVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(apartmentsVM.Pictures);
                var urls = new List<string>();

                foreach (var obj in result)
                {
                    var url = obj.Url.ToString();
                    urls.Add(url);
                }
                var apartments = new Apartments
                {

                    Description = apartmentsVM.Description,
                    ApartmentCategory = apartmentsVM.ApartmentCategory,
                    Price = apartmentsVM.Price,
                    ApartmentPictures = new ApartmentPictures
                    {
                        Pic1 = urls[0],
                        Pic2 = urls[1],
                        Pic3 = urls[2],
                        Pic4 = urls[3],
                        Pic5 = urls[4],
                        Pic6 = urls[5]

                    },
                    Address = new Address
                    {
                        Address_Line1 = apartmentsVM.Address.Address_Line1,
                        Address_Line2 = apartmentsVM.Address.Address_Line2,
                        City = apartmentsVM.Address.City,
                        State = apartmentsVM.Address.State,
                        Zip = apartmentsVM.Address.Zip,
                    }


                };
                _apartmentsRepository.Add(apartments);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");

            }
            return View(apartmentsVM);

        }
        public async Task<IActionResult> AllocateTenant(int id)
        {

            IEnumerable<AppUser> tenants = await _apartmentsRepository.GetAllTenants();

            var allocateUserVM = new AllocateTenantViewModel
            {
                ApartmetId = id,
                AppUsers = (List<AppUser>)tenants,
            };
            return View(allocateUserVM);
        }
        [HttpPost]
        public async Task<IActionResult> AllocateTenant(int id, AllocateTenantViewModel allocateUserVM)
        {
            var user = await _apartmentsRepository.GetUserByName(allocateUserVM.UserName);
            var apartment = await _apartmentsRepository.GetByIdAsync(id);
            apartment.UserId = user.Id;

            _apartmentsRepository.Update(apartment);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var apartment = await _apartmentsRepository.GetByIdAsync(id);
            if (apartment == null) return View("Error");
            var apartmentVM = new EditApartmentViewModel
            {
                ApartmentCategory = apartment.ApartmentCategory,
                Description = apartment.Description,
                Price = apartment.Price,
                AddressId = apartment.AddressId,
                Address = apartment.Address,
                ApartmentPictures = apartment.ApartmentPictures

            };
            return View(apartmentVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApartmentViewModel apartmentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit apartment");
                return View("Edit", apartmentVM);
            }

            var userApartment = await _apartmentsRepository.GetByIdAsyncNoTracking(id);

            if (userApartment != null)
            {
                try
                {
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic1);
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic2);
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic3);
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic4);
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic5);
                    await _photoService.DeletePhotonsAsync(userApartment.ApartmentPictures.Pic6);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(apartmentVM);
                }
                var result = await _photoService.AddPhotoAsync(apartmentVM.Pictures);
                var urls = new List<string>();

                foreach (var obj in result)
                {
                    var url = obj.Url.ToString();
                    urls.Add(url);
                }
                var apartment = new Apartments
                {
                    Id = id,
                    Description = apartmentVM.Description,
                    Price = apartmentVM.Price,
                    ApartmentPictureId = apartmentVM.PicturesId,
                    ApartmentPictures = apartmentVM.ApartmentPictures,
                    AddressId = apartmentVM.AddressId,
                    Address = apartmentVM.Address,
                };

                _apartmentsRepository.Update(apartment);
                return RedirectToAction("Index");
            }
            else
            {
                return View(apartmentVM);
            }

        }
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var apartmentDetails = await _apartmentsRepository.GetByIdAsync(id);
            if (apartmentDetails == null) return View("Error");

            _apartmentsRepository.DeleteApartment(apartmentDetails);
            return RedirectToAction("Index");
        }
    }
}

