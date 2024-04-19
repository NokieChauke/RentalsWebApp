using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IDocumentsRepository
    {
        Task<Documents> GetUploadedDocuments(string id);
        bool Add(Documents document);
        bool Update(Documents document);
        bool Delete(Documents document);
        bool Save();

    }
}
