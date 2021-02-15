namespace SemesterProjectManager.Web.ViewModels
{
	using System.ComponentModel.DataAnnotations;

	public class EditTaskViewModel : CreateTaskViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Approve")]
		public bool IsApproved { get; set; }
	}
}
