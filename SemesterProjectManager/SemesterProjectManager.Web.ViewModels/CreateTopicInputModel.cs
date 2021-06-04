namespace SemesterProjectManager.Web.ViewModels
{
	using System.ComponentModel.DataAnnotations;

	public class CreateTopicInputModel
	{
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Requirements")]
		public string Requirements { get; set; }

		public int SubjectId { get; set; }
	}
}
