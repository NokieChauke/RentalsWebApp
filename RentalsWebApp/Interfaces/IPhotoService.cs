using CloudinaryDotNet.Actions;

namespace RentalsWebApp.Interfaces
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotonsAsync(string publicId);
	}
}
