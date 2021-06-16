namespace SemesterProjectManager.Services
{
	using Microsoft.AspNetCore.Identity;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;
	using SemesterProjectManager.Web.ViewModels;

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

		public IEnumerable<ApplicationUser> GetAllUsers()
		{
			var users = this.context.Users.Select(x => x);

			return users;
		}

		public ASYNC.Task<IList<ApplicationUser>> GetStudents()
		{
			throw new NotImplementedException();
		}

		public async ASYNC.Task<IEnumerable<ApplicationUser>> GetTeachers()
		{
			var teachers = await this.userManager.GetUsersInRoleAsync(AccountType.Teacher.ToString());
			return teachers;
		}

		public async ASYNC.Task<CreateSubjectInputModel> GetTeachersFullNameWithId()
		{
			var teachers = await this.GetTeachers();

			var model = new CreateSubjectInputModel()
			{
				Teachers = teachers.ToDictionary(x => x.Id, x => $"{x.Title} {x.FirstName} {x.LastName}")
			};

			return model;
		}

		public async ASYNC.Task<ApplicationUser> GetUserById(string id)
		{
			var user = await this.userManager.FindByIdAsync(id);

			return user;
		}
	}
}
