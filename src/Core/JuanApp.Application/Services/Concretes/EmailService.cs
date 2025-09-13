using JuanApp.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace JuanApp.Application.Services.Concretes;

public class EmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("allupproje@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new TextPart(TextFormat.Html) { Text = body };
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("allupproje@gmail.com", "pqrf fcbl fmvy kkzf");
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
