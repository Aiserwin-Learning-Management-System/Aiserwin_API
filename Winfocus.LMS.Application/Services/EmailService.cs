namespace Winfocus.LMS.Application.Services
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using Winfocus.LMS.Application.Configuration;
    using Winfocus.LMS.Application.Helpers;
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
            var activationLink =
                $"{_smtpSettings.Domain}/activate-account?token={token}";

            var htmlBody = EmailTemplateRenderer.Render(
                "activation.html",
                new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "ACTION_LINK", activationLink },
                    { "BUTTON_TEXT", "Set Password" },
                });

            var message = BuildMessage(
                email,
                "AISERWIN Registration Successful",
                htmlBody);

            await SendAsync(message);

            _logger.LogInformation(
                "Activation email sent successfully to {Email}",
                email);
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
            var resetLink =
                $"{_smtpSettings.Domain}/reset-password?token={token}";

            var htmlBody = EmailTemplateRenderer.Render(
                "reset-password.html",
                new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "ACTION_LINK", resetLink },
                    { "BUTTON_TEXT", "Reset Password" },
                });

            var message = BuildMessage(
                email,
                "Reset Your AISERWIN Password",
                htmlBody);

            await SendAsync(message);

            _logger.LogInformation(
                "Reset password email sent successfully to {Email}",
                email);
        }

        private MimeMessage BuildMessage(
            string toEmail,
            string subject,
            string htmlBody)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _smtpSettings.FromName,
                _smtpSettings.FromEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = "Please view this email in an HTML-compatible client.",
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
