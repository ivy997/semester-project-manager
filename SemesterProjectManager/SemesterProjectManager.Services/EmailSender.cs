namespace SemesterProjectManager.Services
{
	using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Options;
	using SendGrid;
	using SendGrid.Helpers.Mail;
	using System.Threading.Tasks;

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

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
            var apiKey = config["SPM_API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("avi1999@abv.bg", "Semester Project Manager");
            var to = new EmailAddress(email);
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            //msg.SetClickTracking(false, false);
            await client.SendEmailAsync(msg);
        }
	}
}