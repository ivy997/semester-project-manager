using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SemesterProjectManager.Web.ViewModels
{
	public class EmailViewModel
	{
		public string Id { get; set; }

		public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
