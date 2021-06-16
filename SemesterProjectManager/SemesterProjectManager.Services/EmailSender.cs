namespace SemesterProjectManager.Services
{
    using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Options;
    using SemesterProjectManager.Services;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
	using System.Threading.Tasks;
	using ASYNC = System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration config;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
            IConfiguration config)
        {
            Options = optionsAccessor.Value;
            this.config = config;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public ASYNC.Task Execute(string email, string subject, string message)
        {
            //var client = new SendGridClient(Options.SendGridKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("avi1999@abv.bg", Options.SendGridUser),
            //    Subject = subject,
            //    PlainTextContent = message,
            //    HtmlContent = message
            //};
            //msg.AddTo(new EmailAddress(email));

            //// Disable click tracking.
            //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //msg.SetClickTracking(false, false);

            //return client.SendEmailAsync(msg);

            return SendEmailAsync(subject, message, email);
        }

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
            var apiKey = config["SPM_API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("avi1999@abv.bg", "Admin");
            var to = new EmailAddress(email);
            var plainTextContent = $"Hello, {email.Split('@')[0]}.";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);

            //msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }

		//public ASYNC.Task Execute(string apiKey, string subject, string message, string email)
		//{

		//}
	}
}