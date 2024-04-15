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
                var result = await _photoService.AddPhotoAsync(apartmentsVM.Picture);
                var apartments = new Apartments
                {

                    Description = apartmentsVM.Description,
                    ApartmentCategory = apartmentsVM.ApartmentCategory,
                    Price = apartmentsVM.Price,
                    Picture = result.Url.ToString(),
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
                URL = apartment.Picture,

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
                    await _photoService.DeletePhotonsAsync(userApartment.Picture);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(apartmentVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(apartmentVM.Picture);
                var apartment = new Apartments
                {
                    Id = id,
                    Description = apartmentVM.Description,
                    Price = apartmentVM.Price,
                    Picture = photoResult.Url.ToString(),
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

