using JuanApp.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace JuanApp.Application.Services.Concretes;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("allupproje@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("allupproje@gmail.com", "pqrf fcbl fmvy kkzf");
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
    public async Task SendBulkEmailAsync(List<string> recipients, string subject, string body)
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("allupproje@gmail.com", "pqrf fcbl fmvy kkzf");

        foreach (var recipient in recipients)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("allupproje@gmail.com"));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            await smtp.SendAsync(email);
        }

        await smtp.DisconnectAsync(true);
    }
}
