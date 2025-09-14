namespace JuanApp.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendBulkEmailAsync(List<string> to, string subject, string body);

    }
}
