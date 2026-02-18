namespace Winfocus.LMS.Application.Services
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using Winfocus.LMS.Application.Configuration;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// EmailService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IEmailService" />
    public sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpSettings _smtpSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="smtpOptions">The SMTP options.</param>
        public EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpOptions)
        {
            _logger = logger;
            _smtpSettings = smtpOptions.Value;
        }

        /// <summary>
        /// Sends the activation email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        public async Task SendActivationEmailAsync(
            string email,
            string username,
            string token)
        {
            try
            {
                var resetLink =
                    $"https://localhost:4200/reset-password?token={token}";

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(
                    _smtpSettings.FromName,
                    _smtpSettings.FromEmail));

                message.To.Add(MailboxAddress.Parse(email));

                message.Subject = "Activate Your Account";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif;'>
                            <h2>Hello {username},</h2>
                            <p>Thank you for registering.</p>
                            <p>Please activate your account by clicking the button below:</p>
                            <p>
                                <a href='{resetLink}'
                                   style='display:inline-block;
                                          padding:10px 18px;
                                          background-color:#007bff;
                                          color:#ffffff;
                                          text-decoration:none;
                                          border-radius:4px;'>
                                   Activate Account
                                </a>
                            </p>
                            <p>This link will expire in 24 hours.</p>
                            <br/>
                            <p>If you did not register, please ignore this email.</p>
                        </div>",
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var smtpClient = new SmtpClient();

                smtpClient.Timeout = 10000; // 10 seconds

                await smtpClient.ConnectAsync(
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    _smtpSettings.EnableSsl
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto);

                await smtpClient.AuthenticateAsync(
                    _smtpSettings.Username,
                    _smtpSettings.Password);

                await smtpClient.SendAsync(message);

                await smtpClient.DisconnectAsync(true);

                _logger.LogInformation(
                    "Activation email sent successfully to {Email}",
                    email);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send activation email to {Email}",
                    email);

                throw;
            }
        }
    }
}
