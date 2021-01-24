namespace SemesterProjectManager.Web.ViewModels
{
	using System.ComponentModel.DataAnnotations;

	public class CreateTopicInputModel
	{
		[Required]
		[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		public int SubjectId { get; set; }
	}
}
