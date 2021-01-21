namespace SemesterProjectManager.Services
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Text;
	using System.Threading.Tasks;

	public class UserService : IUserService
	{
		private readonly ApplicationDbContext context;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;

		public UserService(ApplicationDbContext context,
				RoleManager<IdentityRole> roleManager,
				UserManager<ApplicationUser> userManager)
		{
			this.context = context;
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		public Task<IList<ApplicationUser>> GetAllUsers()
		{
			throw new NotImplementedException();
		}

		public Task<int> GetStudentById()
		{
			throw new NotImplementedException();
		}

		public Task<IList<ApplicationUser>> GetStudents()
		{
			throw new NotImplementedException();
		}

		public Task<int> GetTeacherById()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<ApplicationUser>> GetTeachers()
		{
			var teachers = await this.userManager.GetUsersInRoleAsync(AccountType.Teacher.ToString());
			return teachers;
		}

		public async Task<IEnumerable<string>> GetTeachersFullName()
		{
			var teachers = await this.GetTeachers();
			var teachersFullNames = new List<string>();
			string fullName = string.Empty;

			foreach (var teacher in teachers.ToList())
			{
				fullName = teacher.Title + " " + teacher.FirstName + " " + teacher.LastName;
				teachersFullNames.Add(fullName);
				fullName = string.Empty;
			}

			return teachersFullNames;
		}

		public Task<int> GetUserById()
		{
			throw new NotImplementedException();
		}
	}
}
