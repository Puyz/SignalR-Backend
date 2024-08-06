using System.Net;
using System.Net.Mail;

namespace EmailSenderExample
{
    public static class EmailSender
    {
        public static void SendMail(string to, string message)
        {
            NetworkCredential credential = new("example@gmail.com", "pass");
            SmtpClient smtp = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = credential,
            };

            MailAddress sender = new("example@gmail.com", "Display name");
            MailAddress receiver = new(to);

            MailMessage mail = new(sender, receiver)
            {
                Subject = "Subject",
                Body = message
            };

            smtp.Send(mail);

        }
    }
}
