using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SemesterProjectManager.Data.Models;
using SemesterProjectManager.Services;
using SemesterProjectManager.Web.ViewModels;
using System;
using System.Linq;
using System.Text;
using ASYNC = System.Threading.Tasks;

namespace SemesterProjectManager.Controllers
{
	public class ContactController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager; 
		private readonly IUserService userService;
		private readonly IEmailSender emailSender;

		public ContactController(UserManager<ApplicationUser> userManager,
			IUserService userService,
			IEmailSender emailSender)
		{
			this.userManager = userManager;
			this.userService = userService;
			this.emailSender = emailSender;
		}

		// GET: HomeController1
		[Authorize]
		public async ASYNC.Task<IActionResult> Index(string statusMessage = null)
		{
			var user = await this.userManager.GetUserAsync(User);
			var teachers = await this.userService.GetTeachers();

			var model = new ContactViewModel()
			{
				SenderFullName = $"{user.FirstName} {user.LastName}",
				SenderEmail = user.Email,
				Teachers = teachers.ToDictionary(x => x.Id, x => $"{x.Title} {x.FirstName} {x.LastName}"),
				StatusMessage = statusMessage,
			};

			return View("Index", model);
		}

		[HttpPost, ActionName("Index")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> SendEmail(ContactViewModel model)
		{
			var user = await this.userManager.GetUserAsync(User);
			var teacher = await this.userService.GetUserById(model.TeacherId);

			if (!ModelState.IsValid)
			{
				model.StatusMessage = "Couldn't send message, please try again or contact support.";
				return RedirectToAction("Index", "Contact", new { statusMessage = model.StatusMessage });
			}

			var message = new StringBuilder();
			message.AppendLine($"Message from <a>{user.Email}: </a>");
			message.Append($"{model.MessageContent}");

			await this.emailSender.SendEmailAsync(teacher.Email, model.MessageSubject, message.ToString());
			model.StatusMessage = "Message sent successfully!";

			return RedirectToAction("Index", "Contact", new { statusMessage = model.StatusMessage });
		}

		// GET: HomeController1/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: HomeController1/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: HomeController1/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: HomeController1/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: HomeController1/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: HomeController1/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: HomeController1/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
