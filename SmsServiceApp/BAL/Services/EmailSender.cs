using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
