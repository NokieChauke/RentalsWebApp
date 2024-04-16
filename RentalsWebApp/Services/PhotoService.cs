using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RentalsWebApp.Helpers;
using RentalsWebApp.Interfaces;

namespace RentalsWebApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<IEnumerable<ImageUploadResult>> AddPhotoAsync(IFormFileCollection files)
        {
            var uploadResult = new ImageUploadResult();
            var photos = new List<ImageUploadResult>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")

                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    photos.Add(uploadResult);
                }
            }

            return photos;
        }
        public async Task<ImageUploadResult> AddProfilePhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotonsAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var results = await _cloudinary.DestroyAsync(deleteParams);
            return results;
        }
    }
}
