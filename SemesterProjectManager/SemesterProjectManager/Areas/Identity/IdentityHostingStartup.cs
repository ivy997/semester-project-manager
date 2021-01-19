using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SemesterProjectManager.Data;
using SemesterProjectManager.Data.Models;

[assembly: HostingStartup(typeof(SemesterProjectManager.Areas.Identity.IdentityHostingStartup))]
namespace SemesterProjectManager.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(
						context.Configuration.GetConnectionString("DefaultConnection")));

				services.AddIdentity<ApplicationUser, IdentityRole>(
					options => 
					{ 
						options.SignIn.RequireConfirmedAccount = false;
						options.Password.RequiredUniqueChars = 0;
						options.Password.RequireUppercase = false;
						options.Password.RequiredLength = 6;
						options.Password.RequireNonAlphanumeric = false;
						options.Password.RequireLowercase = false;
					})
					.AddDefaultTokenProviders()
					.AddDefaultUI()
					.AddEntityFrameworkStores<ApplicationDbContext>();
			});
		}
	}
}