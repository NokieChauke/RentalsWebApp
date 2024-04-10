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

        public async Task<List<Documents>> GetUploadedDocuments()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var documents = _context.Documents.Where(d => d.AppUser.Id == currentUser);

            return documents.ToList();

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
