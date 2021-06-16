using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SemesterProjectManager.Data.Models;
using SemesterProjectManager.Services;
using SemesterProjectManager.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using ASYNC = System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace SemesterProjectManager.Controllers
{
	public class AdminController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IUserService userService;
		private readonly IEmailSender emailSender;
		private readonly ILogger<AdminController> logger;
		private readonly UrlEncoder urlEncoder;

		private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

		public AdminController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IUserService userService,
			IEmailSender emailSender,
			ILogger<AdminController> logger,
			UrlEncoder urlEncoder)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.userService = userService;
			this.emailSender = emailSender;
			this.logger = logger;
			this.urlEncoder = urlEncoder;
		}

		// GET: List all users
		public ActionResult<IEnumerable<UserViewModel>> Index()
		{
			IEnumerable<UserViewModel> users = this.userService.GetAllUsers().Select(x => new UserViewModel
			{
				Id = x.Id,
				FullName = $"{x.FirstName} {x.LastName}",
				AccountType = x.AccountType,
				Email = x.Email,
				EmailConfirmed = x.EmailConfirmed,
			}); 

			return View("Index", users);
		}

		// GET: HomeController1/Details/5
		[ActionName("Profile")]
		public async ASYNC.Task<IActionResult> ViewProfile(string id, string error = null, string statusMessage = null)
		{
			ApplicationUser user = await this.userService.GetUserById(id);
			var userForView = new ProfileViewModel()
			{
				Id = user.Id,
				Username = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber,
				FacultyNumber = user.FacultyNumber,
				Title = user.Title,
				AccountType = user.AccountType,
				ErrorMessage = error,
				StatusMessage = statusMessage,
			};

			ViewData["UserId"] = user.Id;

			return View("Profile", userForView);
		}

		//[ActionName("EditProfile")]
		[HttpPost, ActionName("Profile")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> EditProfile(string id, ProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// throw exception
			}

			var user = await this.userService.GetUserById(id);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{id}'.");
			}

			var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
			if (model.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					model.StatusMessage = "Unexpected error when trying to set phone number.";
					return RedirectToAction("Profile", "Admin", new { id = id, statusMessage = model.StatusMessage });
				}
			}

			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.FacultyNumber = model.FacultyNumber;
			user.Title = model.Title;
			var setUserData = await this.userManager.UpdateAsync(user);
			if (!setUserData.Succeeded)
			{
				model.StatusMessage = "Unexpected error when trying to update profile info.";
				return RedirectToAction("Profile", "Admin", new { id = id, statusMessage = model.StatusMessage });
			}

			await this.signInManager.RefreshSignInAsync(user);
			model.StatusMessage = "Your profile has been updated";
			return RedirectToAction("Profile", "Admin", new { id = id, statusMessage = model.StatusMessage });
		}

		[ActionName("Email")]
		public async ASYNC.Task<IActionResult> ViewEmail(string id, string statusMessage = null)
		{
			ApplicationUser user = await this.userService.GetUserById(id);

			var userForView = new EmailViewModel()
			{
				Id = user.Id,
				Username = user.UserName,
				StatusMessage = statusMessage,
				Email = user.Email,
				IsEmailConfirmed = user.EmailConfirmed,
			};

			ViewData["UserId"] = user.Id;

			return View("Email", userForView);
		}

		[HttpPost, ActionName("Email")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> EditEmail(string id, EmailViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// throw exception
			}

			var user = await this.userService.GetUserById(id);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{id}'.");
			}

			var email = await this.userManager.GetEmailAsync(user);
			if (model.NewEmail != email)
			{
				var userId = await this.userManager.GetUserIdAsync(user);
				var code = await this.userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
				var callbackUrl = Url.Page(
					"/Account/ConfirmEmailChange",
					pageHandler: null,
					values: new { userId = userId, email = model.NewEmail, code = code },
					protocol: Request.Scheme);
				await this.emailSender.SendEmailAsync(
					model.NewEmail,
					"Confirm your email",
					$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

				model.StatusMessage = "Confirmation link to change email sent. Please check your email.";
				return RedirectToAction("Email", "Admin", new { id = id, statusMessage = model.StatusMessage});
			}

			model.StatusMessage = "Your email is unchanged.";
			return RedirectToAction("Email", "Admin", new { id = id, statusMessage = model.StatusMessage });
		}

		[ActionName("Password")]
		public async ASYNC.Task<IActionResult> ChangePassword(string id, string error = null, string statusMessage = null)
		{
			var user = await this.userService.GetUserById(id);

			var model = new PasswordViewModel()
			{
				StatusMessage = statusMessage,
			};

			ViewData["UserId"] = user.Id;

			return View("Password", model);
		}

		[HttpPost, ActionName("Password")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> ChangePassword(string id, PasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// throw exception
			}

			var user = await this.userService.GetUserById(id);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{id}'.");
			}

			var changePasswordResult = await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				foreach (var error in changePasswordResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				// display errors
				return RedirectToAction("Password", "Admin", new { id = id });
			}

			await this.signInManager.RefreshSignInAsync(user);
			this.logger.LogInformation("User changed their password successfully.");
			model.StatusMessage = "Your password has been changed.";

			return RedirectToAction("Password", "Admin", new { id = id, statusMessage = model.StatusMessage });
		}

	}
}
