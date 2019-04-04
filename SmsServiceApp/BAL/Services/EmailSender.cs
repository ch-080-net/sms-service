using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApp.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var from = "q.u.i.c.k.sender.r.r@gmail.com";
			var pass = "quicksender123";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Credentials = new System.Net.NetworkCredential(from, pass);
			client.EnableSsl = true;
			var mail = new MailMessage(from, email);
			mail.Subject = subject;
			mail.Body = message;
			mail.IsBodyHtml = true;
			return client.SendMailAsync(mail);
		}

        public Task SendEmailsAsync(string from, string to, string message)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 2526,
                Host = "localhost"
            };
            MailMessage email = new MailMessage()
            {
                From = new MailAddress(from, from),
                Body = message,
                IsBodyHtml = true,
                Sender = new MailAddress(from),
            };
            email.To.Add(new MailAddress(to));
            return client.SendMailAsync(email);
        }
	}
}
