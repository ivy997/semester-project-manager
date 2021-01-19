namespace SemesterProjectManager.Web.ViewModels
{
	using SemesterProjectManager.Data.Models;

	public class CreateSubjectInputModel
	{
		public string Name { get; set; }

		public Teacher Teacher { get; set; }
	}
}
