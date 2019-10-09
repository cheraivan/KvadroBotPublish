using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KopterBot.Commons
{
    class Email
    {
        public static async Task SendEmail(string message)
        {
            MailAddress from = new MailAddress("ivan.cherednychenko@gmail.com", "Tom");
            MailAddress to = new MailAddress("somemail@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("ivan.cherednychenko@gmail.com", "cvthnm182123");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}