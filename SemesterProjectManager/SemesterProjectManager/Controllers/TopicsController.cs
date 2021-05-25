namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using SemesterProjectManager.Services;
	using SemesterProjectManager.Web.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class TopicsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITopicService topicService;
		private readonly ITaskService taskService;
		private readonly IUserService userService;
		private readonly IProjectService projectService;
		private readonly ISubjectService subjectService;

		public TopicsController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			ITopicService topicService,
			ITaskService taskService,
			IUserService userService,
			IProjectService projectService,
			ISubjectService subjectService)
		{
			this.context = context;
			this.userManager = userManager;
			this.topicService = topicService;
			this.taskService = taskService;
			this.userService = userService;
			this.projectService = projectService;
			this.subjectService = subjectService;
		}

		[Authorize]
		public ActionResult<IEnumerable<SubjectViewModel>> All()
		{
			var topics = this.topicService.GetAll();

			return this.View("All", topics);
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Create(int subjectId)
		{
			var topicModel = new CreateTopicInputModel();
			topicModel.SubjectId = subjectId;
			return this.View("Create", topicModel);
		}

		[HttpPost]
		[Authorize(Roles = "Teacher")]
		public IActionResult Create(CreateTopicInputModel model)
		{
			if (ModelState.IsValid)
			{
				this.topicService.CreateAsync(model);
			}

			return RedirectToAction("Details", new RouteValueDictionary(new 
			{ 
				controller = "Subjects", 
				action = "Details", 
				Id = model.SubjectId 
			}));
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Edit(int id)
		{
			var topic = await this.topicService.GetById(id);

			if (topic == null)
			{
				return NotFound();
			}

			var topicToEdit = new EditTopicViewModel()
			{
				Id = topic.Id,
				Title = topic.Title,
				Description = topic.Description,
				StateOfApproval = topic.StateOfTopic,
				SubjectId = topic.SubjectId
			};

			return View(topicToEdit);
		}

		[HttpPost, ActionName("Edit")]
		[Authorize(Roles = "Teacher")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(EditTopicViewModel input, int id)
		{
			this.topicService.Edit(input, id);

			return RedirectToAction("Details", new RouteValueDictionary(new 
			{ 
				controller = "Subjects", 
				action = "Details", 
				Id = input.SubjectId 
			}));
		}

		[Authorize(Roles = "Teacher, Student")]
		public async Task<ActionResult<string>> Details(int id)
		{
			var topic = await this.topicService.GetById(id);
			var user = await this.userManager.GetUserAsync(this.User);
			var project = await this.projectService.GetByStudentId(user.Id);
			var subject = await this.subjectService.GetById(topic.SubjectId);
			var teacher = await this.userService.GetUserById(subject.TeacherId);

			if (topic == null)
			{
				return NotFound();
			}

			if (user == null)
			{
				return NotFound();
			}

			var tasks = await this.taskService.GetAllByTopicId(id);
			var tasksForView = new List<TaskViewModel>();
			foreach (var task in tasks)
			{
				var student = await this.userService.GetUserById(task.StudentId);
				var currTask = new TaskViewModel()
				{
					Id = task.Id,
					StudentFullName = $"{student.FirstName} {student.LastName}",
					FacultyNumber = student.FacultyNumber,
					CreatedOn = task.CreatedOn,
					IsApproved = task.IsApproved,
				};
				tasksForView.Add(currTask);
			}

			if (this.User.IsInRole("Student"))
			{
				tasksForView = tasksForView
					.Where(x => x.FacultyNumber == user.FacultyNumber)
					.ToList();
			}

			if (project != null && project.TopicId == topic.Id)
			{
				topic.StateOfTopic = StateOfApproval.Submitted;
			}
			else if (tasks.Any(x => x.StudentId == user.Id) && tasksForView.Any(x => x.IsApproved == true))
			{
				topic.StateOfTopic = StateOfApproval.Approved;
			}
			else if (tasks.Any(x => x.StudentId == user.Id) && tasksForView.All(x => x.IsApproved == false))
			{
				topic.StateOfTopic = StateOfApproval.PendingApproval;
			}
			else
			{
				topic.StateOfTopic = StateOfApproval.Available;
			}

			var topicViewModel = new EditTopicViewModel()
			{
				Id = topic.Id,
				Title = topic.Title,
				Description = topic.Description,
				StateOfApproval = topic.StateOfTopic,
				SubjectId = topic.SubjectId,
				Tasks = tasksForView,
				FacultyNumber = user.FacultyNumber,
				TeacherFullName = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
			};

			return View(topicViewModel);
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
		{
			var topic = await this.topicService.GetById(id);

			if (topic == null)
			{
				return NotFound();
			}

			var topicToDelete = new EditTopicViewModel()
			{
				Id = topic.Id,
				Title = topic.Title,
				Description = topic.Description,
				StateOfApproval = topic.StateOfTopic,
				SubjectId = topic.SubjectId
			};

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(topicToDelete);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Teacher")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id, int subjectId)
		{
			try
			{
				await this.topicService.Delete(id);
				return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Subjects", action = "Details", Id = subjectId }));
				//return RedirectToAction("All", "Subjects");
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
			}
		}
	}
}
