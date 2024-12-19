using System.Net;
using System.Net.Mail;
using DoctorOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(email)) 
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        if (string.IsNullOrWhiteSpace(subject)) 
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));
        if (string.IsNullOrWhiteSpace(htmlMessage)) 
            throw new ArgumentException("Message body cannot be null or empty", nameof(htmlMessage));

        var host = _configuration["Smtp:Host"] ?? throw new InvalidOperationException("SMTP host is not configured.");
        var port = _configuration["Smtp:Port"] ?? throw new InvalidOperationException("SMTP port is not configured.");
        var username = _configuration["Smtp:Username"] ?? throw new InvalidOperationException("SMTP username is not configured.");
        var password = _configuration["Smtp:Password"] ?? throw new InvalidOperationException("SMTP password is not configured.");
        var from = _configuration["Smtp:From"] ?? throw new InvalidOperationException("SMTP from address is not configured.");

        using var smtpClient = new SmtpClient(host)
        {
            Port = int.Parse(port),
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        try
        {
            await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send email.", ex);
        }
    }
}
