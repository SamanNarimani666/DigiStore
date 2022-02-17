using System.Net.Mail;
namespace DigiStore.Application.Senders
{
    public interface ISender
    {
        bool SendEmail(string to, string subject, string body);
    }
    public class EmailSender:ISender
    {
        public bool SendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.mail.yahoo.com");
            mail.From = new MailAddress("Samannarimani666@yahoo.com", "ديجي استور");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;

            SmtpServer.Credentials = new System.Net.NetworkCredential("Samannarimani666@yahoo.com","******");
            SmtpServer.Send(mail);
            return true;
        }
    }
}
