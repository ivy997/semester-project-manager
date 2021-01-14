namespace SemesterProjectManager.Data.Models
{
	public class Student : ApplicationUser
	{
		public int FacultyNumber { get; set; }

		//public int TopicId { get; set; }

		//public Topic Topic { get; set; }

		public int TaskId { get; set; }

		public Task Task { get; set; }

		public Project Project { get; set; }
	}
}
