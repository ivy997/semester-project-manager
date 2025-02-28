﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SemesterProjectManager.Data.Models;
using SemesterProjectManager.Services;
using System.ComponentModel.DataAnnotations;
using ASYNC = System.Threading.Tasks;

namespace SemesterProjectManager.Areas.Identity.Pages.Account.Manage
{
	public partial class IndexModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserService _userService;

		public IndexModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IUserService userService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_userService = userService;
		}

		public string Username { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "First name")]
			public string FirstName { get; set; }

			[Display(Name = "Last name")]
			public string LastName { get; set; }

			[Display(Name = "Faculty number")]
			
			public int FacultyNumber { get; set; }

			[Display(Name = "Teacher titles")]
			public string Title { get; set; }

			[Phone]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }

			public string ErrorMessage { get; set; }
		}

		private async ASYNC.Task LoadAsync(ApplicationUser user, string error = null)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;

			Input = new InputModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				FacultyNumber = user.FacultyNumber,
				Title = user.Title,
				PhoneNumber = phoneNumber,
				ErrorMessage = error
			};
		}

		public async ASYNC.Task<IActionResult> OnGetAsync(string id)
		{
			var user = await _userManager.GetUserAsync(User);
			//var user = await _userService.GetUserById(id);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
				//return NotFound($"Unable to load user with ID '{id}'.");
			}

			await LoadAsync(user);
			return Page();
		}

		public async ASYNC.Task<IActionResult> OnPostAsync(string id)
		{
			var user = await _userManager.GetUserAsync(User);
			//var user = await _userService.GetUserById(id);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
				//return NotFound($"Unable to load user with ID '{id}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}

			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
			if (Input.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set phone number.";
					return RedirectToPage();
				}
            }

			//var userWithFacultyNumber = _userManager.Users.FirstOrDefault(x => x.FacultyNumber == Input.FacultyNumber);
			//if (userWithFacultyNumber != null && await _userManager.IsInRoleAsync(userWithFacultyNumber, "Student"))
			//{
			//	string errorMessage = $"There is already a user with faculty number {Input.FacultyNumber}";
			//	await LoadAsync(user, errorMessage);
			//	return Page();
			//}

			user.FirstName = Input.FirstName;
			user.LastName = Input.LastName;
			user.FacultyNumber = Input.FacultyNumber;
			user.Title = Input.Title;
			var setUserData = await _userManager.UpdateAsync(user);
			if (!setUserData.Succeeded)
			{
				StatusMessage = "Unexpected error when trying to update profile info.";
				return RedirectToPage();
			}

			await _signInManager.RefreshSignInAsync(user);
			StatusMessage = "Your profile has been updated";
			return RedirectToPage();
		}
	}
}
