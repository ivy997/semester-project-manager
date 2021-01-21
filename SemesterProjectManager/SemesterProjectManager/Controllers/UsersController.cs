namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using System;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	public class UsersController : Controller
	{
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;

		public UsersController(RoleManager<IdentityRole> roleManager,
				UserManager<ApplicationUser> userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		//public async ASYNC.Task<ActionResult<string>> Test()
		//{
		//	var user = await this.userManager.GetUserAsync(this.User);
		//	var roles = await this.userManager.GetRolesAsync(user);
		//	return String.Join(',', roles);
		//}
	}
}
