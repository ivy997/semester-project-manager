namespace SemesterProjectManager.Web.ViewModels
{
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using System;
	using System.ComponentModel.DataAnnotations;

	public class ProjectViewModel : PageModel
	{
		

		public int Id { get; set; }

		public int TopicId { get; set; }

		[Display(Name = "Topic")]
		public string TopicName { get; set; }

		public string StudentId { get; set; }

		[Display(Name = "Student")]
		public string StudentFullName { get; set; }

		[Display(Name = "Faculty number")]
		public int FacultyNumber { get; set; }

		[Display(Name = "File")]
		public string FileName { get; set; }

		[Display(Name = "Created on")]
		public DateTime CreatedOn { get; set; }

		[Display(Name = "Grade")]
		public int Score { get; set; }
	}
}
