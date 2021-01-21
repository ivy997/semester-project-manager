namespace SemesterProjectManager.Web.ViewModels
{
	using SemesterProjectManager.Data.Models;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateSubjectInputModel
	{
		[Required]
		[StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
		[Display(Name = "Subject name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Teacher")]
		public ApplicationUser Teacher { get; set; }

		public IEnumerable<string> TeachersList { get; set; }
	}
}
