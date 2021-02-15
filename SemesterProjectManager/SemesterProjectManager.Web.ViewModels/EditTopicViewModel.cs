namespace SemesterProjectManager.Web.ViewModels
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;

	public class EditTopicViewModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "State of approval")]
		[EnumDataType(typeof(StateOfApproval))]
		public StateOfApproval StateOfApproval { get; set; }

		public int SubjectId { get; set; }

		public IEnumerable<TaskViewModel> Tasks { get; set; }

		public ProjectViewModel Project { get; set; }

		//public BufferedSingleFileUploadDbModel FileUpload { get; set; }

		//public CreateTaskViewModel CreateTask { get; set; }
	}
}
