namespace SemesterProjectManager.Controllers
{
	using System;
	using System.Linq;
	using ASYNC = System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	using SemesterProjectManager.Services;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Authorization;
	using System.Collections.Generic;

	public class TasksController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ISubjectService subjectService;
		private readonly ITopicService topicService;
		private readonly ITaskService taskService;
		private readonly IUserService userService;

		public TasksController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			ISubjectService subjectService,
			ITopicService topicService,
			ITaskService taskService,
			IUserService userService)
		{
			this.context = context;
			this.userManager = userManager;
			this.subjectService = subjectService;
			this.topicService = topicService;
			this.taskService = taskService;
			this.userService = userService;
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public ActionResult<IEnumerable<TaskViewModel>> All()
		{
			var tasks = this.taskService.GetAll().Select(x => new TaskViewModel()
			{
				Id = x.Id,
				StudentFullName = $"{this.userService.GetUserById(x.StudentId).Result.FirstName} " +
								  $"{this.userService.GetUserById(x.StudentId).Result.LastName}",
				FacultyNumber = this.userService.GetUserById(x.StudentId).Result.FacultyNumber,
				TopicName = this.topicService.GetById(x.TopicId).Result.Title,
				SubjectName = this.subjectService.GetById(this.topicService.GetById(x.TopicId).Result.SubjectId).Result.Name,
				CreatedOn = x.CreatedOn.ToLocalTime(),
				IsApproved = x.IsApproved,
			});

			return this.View("All", tasks);
		}

		[Authorize(Roles = "Admin, Support, Student")]
		public async ASYNC.Task<IActionResult> Create(int id, int subjectId)
		{
			var user = await this.userManager.GetUserAsync(this.User);

			var taskFromStudent = this.taskService.GetAllByTopicId(id).Result.Any(x => x.StudentId == user.Id);

			if (taskFromStudent)
			{
				return RedirectToAction("Details", new RouteValueDictionary(new 
				{ 
					controller = "Topics", 
					action = "Details", 
					Id = id 
				}));
			}

			var taskModel = new CreateTaskViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				FacultyNumber = user.FacultyNumber,
				CreatedOn = DateTime.Now.Date.ToLocalTime(),
				DueDate = DateTime.Now.Date.AddDays(60),
				StudentId = user.Id,
				SubjectId = subjectId,
				TopicId = id,
			};

			return this.View("Create", taskModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Support, Student")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> Create(CreateTaskViewModel model)
		{
			if (ModelState.IsValid)
			{
				await this.taskService.Create(model);
			}

			return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Topics", action = "Details", Id = model.TopicId }));
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<IActionResult> Edit(int id)
		{
			var task = await this.taskService.GetById(id);
			var student = await this.userService.GetUserById(task.StudentId);

			if (task == null)
			{
				return NotFound();
			}

			var taskModel = new EditTaskViewModel()
			{
				Id = task.Id,
				FirstName = student.FirstName,
				LastName = student.LastName,
				FacultyNumber = student.FacultyNumber,
				MainTask = task.MainTask,
				OutputData = task.OutputData,
				CreatedOn = task.CreatedOn,
				DueDate = task.DueDate,
				IsApproved = task.IsApproved,
				StudentId = task.StudentId,
				TopicId = task.TopicId,
			};

			return View(taskModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Support, Teacher")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> Edit(EditTaskViewModel model, int id)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await this.taskService.Edit(model, id);
				}
				catch (DbUpdateConcurrencyException)
				{
					//Handle error
					throw;
				}
			}

			return RedirectToAction("Details", new RouteValueDictionary(new 
			{ 
				controller = "Topics", 
				action = "Details", 
				Id = model.TopicId 
			}));
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<IActionResult> Delete(int id)
		{
			var task = await this.taskService.GetById(id);
			var student = await this.userService.GetUserById(task.StudentId);

			if (task == null)
			{
				return NotFound();
			}

			var taskToDelete = new DeleteTaskViewModel()
			{
				Id = task.Id,
				StudentFullName = $"{student.FirstName} {student.LastName}",
				FacultyNumber = student.FacultyNumber,
				CreatedOn = task.CreatedOn,
				TopicId = task.TopicId,
			};

			return this.View("Delete", taskToDelete);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin, Support, Teacher")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> DeleteConfirmed(int id, int topicId)
		{
			await this.taskService.Delete(id);

			return RedirectToAction("Details", new RouteValueDictionary(new 
			{ 
				controller = "Topics", 
				action = "Details", 
				Id = topicId 
			}));
		}
	}
}
