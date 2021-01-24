namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Services;
	using SemesterProjectManager.Web.ViewModels;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

	public class SubjectsController : Controller
	{
		private readonly ISubjectService subjectService;
		private readonly IUserService userService;
		private readonly ITopicService topicService;
		private readonly ApplicationDbContext context;

		public SubjectsController(ISubjectService subjectService,
				IUserService userService,
				ITopicService topicService,
				ApplicationDbContext context)
		{
			this.subjectService = subjectService;
			this.userService = userService;
			this.topicService = topicService;
			this.context = context;
		}

		[Authorize]
		public ActionResult<IEnumerable<SubjectViewModel>> All()
		{
			// Find a better way to get teacher's full name

			var subjects = this.subjectService.GetAll().Select(x => new SubjectViewModel
			{
				Id = x.Id,
				Name = x.Name,
				TeacherFullName = $"{this.userService.GetUserById(x.TeacherId).Result.Title}" + " " +
								  $"{this.userService.GetUserById(x.TeacherId).Result.FirstName}" + " " +
								  $"{this.userService.GetUserById(x.TeacherId).Result.LastName}"
			});

			return this.View("All", subjects);
		}

		public async Task<IActionResult> Create()
		{
			var teachersModel = await this.userService.GetTeachersFullNameWithId();
			return this.View("Create", teachersModel);
		}

		[HttpPost]
		public IActionResult Create(CreateSubjectInputModel model)
		{
			if (ModelState.IsValid)
			{
				this.subjectService.CreateAsync(model);
			}

			return this.Redirect("/Subjects/All");
		}

		public async Task<ActionResult<string>> Details(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var subjectModel = await this.subjectService.Details(id);

			if (subjectModel == null)
			{
				return NotFound();
			}

			var teacher = await this.userService.GetUserById(subjectModel.TeacherId);

			var subjectViewModel = new SubjectDetailsViewModel()
			{
				Id = subjectModel.Id,
				Name = subjectModel.Name,
				TeacherFullName = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
				Topics = await this.topicService.GetAllBySubjectId(subjectModel.Id),
			};

			return View(subjectViewModel);
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var subject = await this.subjectService.GetById(id);

			if (subject == null)
			{
				return NotFound();
			}

			// Make a new serviceModel for this
			// Transfer this code to the service
			var subjectToEdit = await this.userService.GetTeachersFullNameWithId();

			subjectToEdit.Id = subject.Id;
			subjectToEdit.Name = subject.Name;
			subjectToEdit.TeacherId = subject.TeacherId;

			return View(subjectToEdit);
		}

		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(CreateSubjectInputModel input, int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			this.subjectService.Edit(input, id);

			return RedirectToAction(nameof(All));
		}

		public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
		{
			var subject = await this.subjectService.Delete(id);
			var teacher = await this.userService.GetUserById(subject.TeacherId);

			if (subject == null)
			{
				return NotFound();
			}

			var subjectToDelete = new SubjectViewModel()
			{
				Id = subject.Id,
				Name = subject.Name,
				TeacherFullName = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
			};

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(subjectToDelete);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				await this.subjectService.DeleteConfirmed(id);
				return RedirectToAction(nameof(All));
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
			}
		}
	}
}
