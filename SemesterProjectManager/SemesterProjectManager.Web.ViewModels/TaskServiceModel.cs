namespace SemesterProjectManager.Web.ViewModels
{
	using System;

	public class TaskServiceModel
	{
		public int Id { get; set; }

		public string StudentId { get; set; }

		public int TopicId { get; set; }

		public DateTime CreatedOn { get; set; }

		public bool IsApproved { get; set; }
	}
}
