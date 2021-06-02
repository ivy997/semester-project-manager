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
			this.Tasks = new HashSet<Task>();
		}

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string Requirements { get; set; }

		public StateOfApproval StateOfTopic { get; set; }

		public int SubjectId { get; set; }

		public Subject Subject { get; set; }

		public string StudentId { get; set; }

		public ApplicationUser Student { get; set; }

		public int? ProjectId { get; set; }

		public Project Project { get; set; }

		// If the Topic the student chose is not approved
		public DateTime? ExpirationDate { get; set; }

		public ICollection<Task> Tasks { get; set; } 
	}
}
