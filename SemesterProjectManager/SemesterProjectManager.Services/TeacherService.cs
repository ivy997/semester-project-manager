namespace SemesterProjectManager.Services
{
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class TeacherService
	{
		private readonly ApplicationDbContext context;

		public TeacherService(ApplicationDbContext context)
		{
			this.context = context;
		}

	}
}
