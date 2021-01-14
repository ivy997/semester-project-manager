namespace SemesterProjectManager.Data.Models
{
	using System.Collections.Generic;

	public class Teacher : ApplicationUser
	{
		public Teacher()
		{
			this.Subjects = new HashSet<Subject>();
		}

		public string Title { get; set; }

		public ICollection<Subject> Subjects { get; set; }
	}
}
