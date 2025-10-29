namespace UserManagement.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendEmailsAsync(List<string> toEmails, string subject, string message);
        Task SendProfileCompletionEmail(string toEmail, string FirstName);
        Task SendUserRegistrationEmail(string toEmail);

        //Task SendPasswordResetEmailAsync(string toEmail, string resetLink);

        //Task SendNotificationEmailAsync(string toEmail, string subject, string message);

    }
}
