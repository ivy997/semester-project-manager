using Microsoft.AspNetCore.Identity;
using SemesterProjectManager.Data.Models;
using SemesterProjectManager.Data.Models.Enums;

namespace SemesterProjectManager.Data.Data
{
	public class DbSeed
	{
		public static void SeedData(UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			SeedRoles(roleManager);
			SeedUsers(userManager);
		}

		public static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExistsAsync("Admin").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "Admin";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}

			if (!roleManager.RoleExistsAsync("Support").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "Support";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}
		}

		public static void SeedUsers(UserManager<ApplicationUser> userManager)
		{
			if (userManager.FindByNameAsync("support@test.test").Result == null)
			{
				ApplicationUser user = new ApplicationUser()
				{
					UserName = "support@test.test",
					FirstName = "Support",
					LastName = "Test",
					Email = "support@test.test",
					AccountType = AccountType.Support,
				};

				IdentityResult result = userManager.CreateAsync(user, "123456").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Support").Wait();
				}
			}

			if (userManager.FindByNameAsync("admin@gmail.com").Result == null)
			{
				ApplicationUser user = new ApplicationUser()
				{
					UserName = "admin@gmail.com",
					Email = "admin@gmail.com",
					FirstName = "Admin",
					LastName = "Test",
					AccountType = AccountType.Admin,
				};

				IdentityResult result = userManager.CreateAsync(user, "123456").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Admin").Wait();
				}
			}
		}
	}
}
