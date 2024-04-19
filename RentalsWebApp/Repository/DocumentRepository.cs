using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class DocumentRepository : IDocumentsRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentRepository(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool Add(Documents documents)
        {
            _context.Add(documents);
            return Save();
        }

        public bool Delete(Documents documents)
        {
            _context.Remove(documents);
            return Save();
        }

        public async Task<Documents> GetUploadedDocuments(string id)
        {
            return await _context.Documents.FirstOrDefaultAsync(d => d.AppUser.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Documents documents)
        {
            _context.Update(documents);
            return Save();
        }
    }
}
