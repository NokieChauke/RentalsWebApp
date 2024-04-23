using RentalsWebApp.Interfaces;
using RentalsWebApp.ViewModels;
using System.Net;
using System.Net.Mail;


namespace RentalsWebApp.Services
{
    public class SendMailService : ISendMail
    {
        public Task NewUserEmail(RegisterViewModel user)
        {
            var message = String.Format("Welcome {0} {1} \n Please find your one time password: {2}\n Click  <a href={3}>Here</a> to change. Please note that the password will expire within 24 hours", user.Name, user.Surname, user.Password, "https://localhost:7076/Dashboard/Security");
            var client = new SmtpClient("smtp.gmail.com", 587)
            {

                Credentials = new NetworkCredential("xiwitsimathebula@gmail.com", "ypgzwxfsdqtgonyv"),
                EnableSsl = true
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: "xiwitsimathebula@gmail.com",
                    to: user.EmailAddress,
                    subject: "Login Credentials",
                    body: message));
        }

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
        public Task ReportIncident(ReportIncidentViewModel reportIncidentVM)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {

                Credentials = new NetworkCredential("xiwitsimathebula@gmail.com", "ypgzwxfsdqtgonyv"),
                EnableSsl = true
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: "xiwitsimathebula@gmail.com",
                    to: reportIncidentVM.EmailToId,
                    reportIncidentVM.IncidentCategory,
                    reportIncidentVM.MoreDetails));

        }
    }
}
