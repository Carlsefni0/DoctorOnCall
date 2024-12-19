namespace DoctorOnCall.RepositoryInterfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}