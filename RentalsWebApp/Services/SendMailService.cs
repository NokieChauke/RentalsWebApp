using RentalsWebApp.Interfaces;
using RentalsWebApp.ViewModels;
using System.Net;
using System.Net.Mail;

namespace RentalsWebApp.Services
{
    public class SendMailService : ISendMail
    {
        public Task SendMailAsync(SendMailViewModel sendMailViewModel)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {

                Credentials = new NetworkCredential("xiwitsimathebula@gmail.com", "ypgzwxfsdqtgonyv"),
                EnableSsl = true
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: "xiwitsimathebula@gmail.com",
                    to: sendMailViewModel.EmailToId,
                    sendMailViewModel.EmailSubject,
                    sendMailViewModel.EmailBody));

        }
    }
}
