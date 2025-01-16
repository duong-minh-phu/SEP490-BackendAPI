using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ClassLibrary1.Services
{
    public interface IEmailService
    {
        Task SendNewPasswordEmail(string fullName, string userEmail, string newPassword);
        Task ResendNewPasswordEmail(string fullName, string userEmail, string newPassword);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Gửi mật khẩu mới cho người dùng
        public async Task SendNewPasswordEmail(string fullName, string userEmail, string newPassword)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
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
                        <p style='color: #ffffff;'>We have generated a new password for your Koi Farm Shop account. Please use the following password to log in:</p>
                        <p style='color: #ffffff; text-align: center;'>
                            <strong>{newPassword}</strong>
                        </p>
                        <p style='color: #ffffff;'>Please log in using this new password and change it once you are logged in.</p>
                        <p style='color: #ffffff;'>Thank you,</p>
                        <p style='color: #ffffff;'>The Koi Farm Shop Team</p>
                    </div>
                </body>
                </html>"
            };

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the error if sending email fails
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        // Gửi lại mật khẩu mới cho người dùng (giống như phương thức trên nhưng có thể được gọi lại nếu yêu cầu)
        public async Task ResendNewPasswordEmail(string fullName, string userEmail, string newPassword)
        {
            await SendNewPasswordEmail(fullName, userEmail, newPassword);
        }
    }
}
