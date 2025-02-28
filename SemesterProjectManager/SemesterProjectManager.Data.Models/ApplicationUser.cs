﻿namespace SemesterProjectManager.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Microsoft.AspNetCore.Identity;
	using SemesterProjectManager.Data.Models.Enums;

	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public AccountType AccountType { get; set; }

		public int FacultyNumber { get; set; }

		public int? TopicId { get; set; }

		public Topic Topic { get; set; }

		public int? TaskId { get; set; }

		public Task Task { get; set; }

		public int? ProjectId { get; set; }

		public Project Project { get; set; }

		public string Title { get; set; }

		public ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
	}
}