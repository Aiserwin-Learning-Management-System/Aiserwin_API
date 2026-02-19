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
        public EmailService(
            ILogger<EmailService> logger,
            IOptions<SmtpSettings> smtpOptions)
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
                var activationLink =
                    $"https://localhost:4200/activate-account?token={token}";

                var message = BuildHtmlEmail(
                    email,
                    "Activate Your Account",
                    username,
                    "Activate Account",
                    activationLink,
                    "activate your account");

                await SendAsync(message);

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

        /// <summary>
        /// Sends the reset password email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        public async Task SendResetPasswordEmailAsync(
            string email,
            string username,
            string token)
        {
            try
            {
                var resetLink =
                    $"https://localhost:4200/reset-password?token={token}";

                var message = BuildHtmlEmail(
                    email,
                    "Reset Your Password",
                    username,
                    "Reset Password",
                    resetLink,
                    "reset your password");

                await SendAsync(message);

                _logger.LogInformation(
                    "Reset password email sent successfully to {Email}",
                    email);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send reset password email to {Email}",
                    email);
                throw;
            }
        }

        private MimeMessage BuildHtmlEmail(
            string toEmail,
            string subject,
            string username,
            string buttonText,
            string actionLink,
            string actionDescription)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _smtpSettings.FromName,
                _smtpSettings.FromEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif;'>
                        <h2>Hello {username},</h2>
                        <p>Please click the button below to {actionDescription}:</p>
                        <p>
                            <a href='{actionLink}'
                               style='display:inline-block;
                                      padding:10px 18px;
                                      background-color:#007bff;
                                      color:#ffffff;
                                      text-decoration:none;
                                      border-radius:4px;'>
                               {buttonText}
                            </a>
                        </p>
                        <p>This link will expire in 24 hours.</p>
                        <br/>
                        <p>If you did not request this, please ignore this email.</p>
                    </div>",
            };

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var smtpClient = new SmtpClient();

            smtpClient.Timeout = 10000;

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
        }
    }
}
