using RentalsWebApp.ViewModels;

namespace RentalsWebApp.Interfaces
{
    public interface ISendMail
    {
        Task SendMailAsync(SendMailViewModel sendMailViewModel);
    }
}
