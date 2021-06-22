namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
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

		[Authorize(Roles = "Admin, Support, Teacher, Student")]
		public ActionResult<IEnumerable<SubjectViewModel>> All()
		{
			var subjects = new List<SubjectViewModel>();

			foreach (var subject in this.subjectService.GetAll())
			{
				var currSubject = new SubjectViewModel()
				{
					Id = subject.Id,
					Name = subject.Name,
				};

				if (subject.TeacherId != null)
				{
					currSubject.TeacherFullName = $"{this.userService.GetUserById(subject.TeacherId).Result.Title}" + " " +
												  $"{this.userService.GetUserById(subject.TeacherId).Result.FirstName}" + " " +
												  $"{this.userService.GetUserById(subject.TeacherId).Result.LastName}";
				}

				subjects.Add(currSubject);
			}

			return this.View("All", subjects);
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async Task<IActionResult> Create()
		{
			var teachersModel = await this.userService.GetTeachersFullNameWithId();
			return this.View("Create", teachersModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Support, Teacher")]
		public IActionResult Create(CreateSubjectInputModel model)
		{
			if (ModelState.IsValid)
			{
				this.subjectService.CreateAsync(model);
			}

			return this.Redirect("/Subjects/All");
		}

		[Authorize(Roles = "Admin, Support, Teacher, Student")]
		public async Task<ActionResult<string>> Details(int id)
		{
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
				Description = subjectModel.Description,
				TeacherFullName = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
				Topics = await this.topicService.GetAllBySubjectId(subjectModel.Id),
			};

			return View(subjectViewModel);
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async Task<IActionResult> Edit(int id)
		{
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
			subjectToEdit.Description = subject.Description;
			subjectToEdit.TeacherId = subject.TeacherId;

			return View(subjectToEdit);
		}

		[HttpPost, ActionName("Edit")]
		[Authorize(Roles = "Admin, Support, Teacher")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(CreateSubjectInputModel input, int id)
		{
			this.subjectService.Edit(input, id);

			return RedirectToAction(nameof(All));
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
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
				//TeacherFullName = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
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
		[Authorize(Roles = "Admin, Support, Teacher")]
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
