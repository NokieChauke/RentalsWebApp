using CloudinaryDotNet.Actions;

namespace RentalsWebApp.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<ImageUploadResult>> AddPhotoAsync(IFormFileCollection files);
        Task<ImageUploadResult> AddProfilePhoto(IFormFile files);
        Task<DeletionResult> DeletePhotonsAsync(string publicId);
    }
}
