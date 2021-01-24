namespace SemesterProjectManager.Data.Models
{
	public class Project : BaseModel<int>
	{
		public string StudentId { get; set; }

		public ApplicationUser Student { get; set; }

		public Topic Topic { get; set; }

		public byte[] ProjectFile { get; set; }

		public int Score { get; set; }
	}
}
