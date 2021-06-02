namespace SemesterProjectManager.Data.Models.Enums
{
	using System.ComponentModel.DataAnnotations;

	public enum StateOfApproval
	{
		Available = 1,
		[Display(Name = "Pending approval")]
		PendingApproval = 2,
		Approved = 3,
		Blocked = 4,
		[Display(Name = "In progress")]
		InProgress = 5,
		Submitted = 6,
		Expired = 7,
		Unavailable = 8
	}
}
