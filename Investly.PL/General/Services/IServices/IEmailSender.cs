namespace Investly.PL.General.Services.IServices
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string Subject, string htmlBody);
    }
}
