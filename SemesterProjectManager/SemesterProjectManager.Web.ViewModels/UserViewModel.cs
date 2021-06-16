using SemesterProjectManager.Data.Models.Enums;

namespace SemesterProjectManager.Web.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public AccountType AccountType { get; set; }

		public bool EmailConfirmed { get; set; }
	}
}
