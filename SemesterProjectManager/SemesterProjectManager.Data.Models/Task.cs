namespace SemesterProjectManager.Data.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Task : BaseModel<int>
	{
		public Task()
		{
			this.CreatedOn = DateTime.UtcNow.Date;
		}

		public string StudentId { get; set; }

		public ApplicationUser Student{ get; set; }

		public int TopicId { get; set; }

		public Topic Topic { get; set; }

		public string MainTask { get; set; }

		public string OutputData { get; set; }

		[Required]
		public DateTime CreatedOn { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		public bool IsApproved { get; set; }
	}
}
