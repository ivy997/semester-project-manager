namespace SemesterProjectManager.Data.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using SemesterProjectManager.Data.Models.Enums;

	public class Topic : BaseModel<int>
	{
		public Topic()
		{
			this.StateOfTopic = StateOfApproval.Available;
			this.ExpirationDuration = TimeSpan.FromDays(7);
		}

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public StateOfApproval StateOfTopic { get; set; }

		public int SubjectId { get; set; }

		public Subject Subject { get; set; }

		public int TaskId { get; set; }

		public Task Task { get; set; }

		public int ProjectId { get; set; }

		public Project Project { get; set; }

		// If the Topic the student chose is not approved
		public TimeSpan? ExpirationDuration { get; set; }

		//public ICollection<Student> Students { get; set; } 
	}
}
