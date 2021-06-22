using System;

namespace SemesterProjectManager.Data.Models
{
	public class Project : BaseModel<int>
	{
		public Project()
		{
			this.CreatedOn = DateTime.UtcNow.Date;
		}

		public string StudentId { get; set; }

		public ApplicationUser Student { get; set; }

		public int TopicId { get; set; }

		public Topic Topic { get; set; }

		public string FileName { get; set; }
	
		public string FileType { get; set; }

		public byte[] ProjectFile { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.UtcNow.Date;

		public int Score { get; set; }
	}
}
