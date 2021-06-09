namespace SemesterProjectManager.Services
{
    using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Options;
    using SemesterProjectManager.Services;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
	using ASYNC = System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        //private readonly IConfiguration configuration;

        //public EmailSender(IConfiguration configuration,
        //    IOptions<AuthMessageSenderOptions> optionsAccessor)
        //{
        //    Options = optionsAccessor.Value;
        //    this.configuration = configuration;
        //}

        //public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        //public async ASYNC.Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    var apiKey = configuration["SMPApiKey"];
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("avi1999@abv.bg", "Ivelina");
        //    //var subjectTest = "Sending with SendGrid is Fun";
        //    var to = new EmailAddress(email, "Ivan");
        //    var plainTextContent = "Dear user,";
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public ASYNC.Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public ASYNC.Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Joe@contoso.com", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}