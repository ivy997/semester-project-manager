namespace SemesterProjectManager.Web.ViewModels
{
	using System.ComponentModel.DataAnnotations;

	public class SubjectViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Subject")]
		public string Name { get; set; }

		[Display(Name = "Subject Summary")]
		public string Description { get; set; }

		[Display(Name = "Teacher")]
		public string TeacherFullName { get; set; }
	}
}
