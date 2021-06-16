using Microsoft.AspNetCore.Mvc;
using SemesterProjectManager.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SemesterProjectManager.Web.ViewModels
{
	public class ProfileViewModel
	{
		public string Id { get; set; }

		[Display(Name = "Email")]
		public string Username { get; set; }

		// This property is only on post
		[TempData]
		public string StatusMessage { get; set; }

		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Display(Name = "User role")]
		public AccountType AccountType { get; set; }

		[Display(Name = "Faculty number")]

		public int FacultyNumber { get; set; }

		[Display(Name = "Teacher titles")]
		public string Title { get; set; }

		[Phone]
		[Display(Name = "Phone number")]
		public string PhoneNumber { get; set; }

		public string ErrorMessage { get; set; }
	}
}
