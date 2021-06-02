namespace SemesterProjectManager.Web.ViewModels
{
	using SemesterProjectManager.Data.Models;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateSubjectInputModel
	{
		// Added id for edit viewmodel
		public int Id { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
		[Display(Name = "Subject name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Teacher")]
		public string TeacherId { get; set; }

		public IDictionary<string, string> Teachers { get; set; }
	}
}
