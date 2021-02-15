namespace SemesterProjectManager.Web.ViewModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class TaskViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Student")]
		public string StudentFullName { get; set; }

		[Display(Name = "Faculty number")]
		public int FacultyNumber { get; set; }

		[Display(Name = "Topic")]
		public string TopicName { get; set; }

		[Display(Name = "Created on")]
		public DateTime CreatedOn { get; set; }

		[Display(Name = "Approved")]
		public bool IsApproved { get; set; }
	}
}
