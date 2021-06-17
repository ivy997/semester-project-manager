using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SemesterProjectManager.Web.ViewModels
{
	public class ContactViewModel
	{
		public string SenderFullName { get; set; }

		public string SenderEmail { get; set; }

		public string StatusMessage { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Subject")]
		public string MessageSubject { get; set; }

		[Required]
		[StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Message")]
		public string MessageContent { get; set; }

		[Required]
		[Display(Name = "Teacher")]
		public string TeacherId { get; set; }

		public IDictionary<string, string> Teachers { get; set; }
	}
}
