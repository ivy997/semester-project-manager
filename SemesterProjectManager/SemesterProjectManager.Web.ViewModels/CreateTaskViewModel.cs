namespace SemesterProjectManager.Web.ViewModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class CreateTaskViewModel
	{
		[Required]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "Faculty number")]
		public int FacultyNumber { get; set; }

		public int TopicId { get; set; }

		public string StudentId { get; set; }

		[Display(Name = "Main task")]
		public string MainTask { get; set; }

		[Display(Name = "Output data")]
		public string OutputData { get; set; }

		[Required]
		[Display(Name = "Created on")]
		public DateTime CreatedOn { get; set; }

		[Required]
		[Display(Name = "Due to")]
		public DateTime DueDate { get; set; }

		public int SubjectId { get; set; }
	}
}
