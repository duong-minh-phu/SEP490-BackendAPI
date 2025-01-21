using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace ClassLibrary1.Services
{
    public interface IEmailService
    {
        Task SendNewPasswordEmail(string fullName, string userEmail, string newPassword);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Gửi mật khẩu mới cho người dùng
        public async Task SendNewPasswordEmail(string fullName, string userEmail, string newPassword)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(newPassword))
            {
                _logger.LogError("Invalid parameters: fullName, userEmail or newPassword is null or empty.");
                throw new ArgumentException("FullName, UserEmail, and NewPassword must be provided.");
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:FromEmail"]));  // Sender's email
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[FU Laptop Sharing] - Your New Password";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Your New Password</title>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #f0f0f0; color: #333333;'>
            <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4CAF50;'>
                <h1 style='color: #ffffff;'>Your New Password</h1>
                <p style='color: #ffffff;'>Hi {fullName},</p>
                <p style='color: #ffffff;'>We have generated a new password for your account. Please use the following password to log in:</p>
                <p style='color: #ffffff; text-align: center;'>
                    <strong>{newPassword}</strong>
                </p>
                <p style='color: #ffffff;'>Please log in using this new password and change it once you are logged in.</p>
                <p style='color: #ffffff;'>Thank you,</p>
                <p style='color: #ffffff;'>The FU Laptop Sharing Team</p>
            </div>
        </body>
        </html>"
            };

            try
            {
                // Get SMTP server configurations from the appsettings.json
                string smtpHost = _config["SmtpSettings:Host"];
                int smtpPort = Convert.ToInt32(_config["SmtpSettings:Port"]);  // Directly cast to int
                string smtpPassword = _config["SmtpSettings:Password"];
                string smtpFromEmail = _config["SmtpSettings:FromEmail"];

                // Validate SMTP settings
                if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(smtpFromEmail))
                {
                    _logger.LogError("SMTP settings are incomplete.");
                    throw new InvalidOperationException("SMTP settings are not configured correctly.");
                }

                // Connect and send the email via SMTP
                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);  // Connect to SMTP server with TLS
                    await smtp.AuthenticateAsync(smtpFromEmail, smtpPassword);  // Authenticate with SMTP server
                    await smtp.SendAsync(email);  // Send the email
                    await smtp.DisconnectAsync(true);  // Disconnect after sending
                }

                _logger.LogInformation($"Password reset email sent to {userEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                throw new InvalidOperationException("An error occurred while sending the password reset email.", ex);
            }
        }

    }
}
