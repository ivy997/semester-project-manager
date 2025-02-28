﻿namespace SemesterProjectManager.Controllers
{
	using ASYNC = System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Services;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using SemesterProjectManager.Web.ViewModels;
	using Microsoft.AspNetCore.Authorization;
	using System;

	public class ProjectsController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ApplicationDbContext context;
		private readonly ISubjectService subjectService;
		private readonly IUserService userService;
		private readonly ITopicService topicService;
		private readonly IProjectService projectService;

		public ProjectsController(ISubjectService subjectService,
				IUserService userService,
				ITopicService topicService,
				IProjectService projectService,
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager)
		{
			this.subjectService = subjectService;
			this.userService = userService;
			this.topicService = topicService;
			this.projectService = projectService;
			this.context = context;
			this.userManager = userManager;
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<ActionResult<IEnumerable<ProjectViewModel>>> All()
		{
			var projectsFromDb = await this.projectService.GetAll();
			var projects = new List<ProjectViewModel>();

			foreach (var project in projectsFromDb)
			{
				var topic = await this.topicService.GetById(project.TopicId);
				var student = await this.userService.GetUserById(project.StudentId);

				var currProject = new ProjectViewModel()
				{
					Id = project.Id,
					TopicName = topic.Title,
					StudentFullName = $"{student.FirstName} {student.LastName}",
					FacultyNumber = student.FacultyNumber,
					FileName = project.FileName,
					CreatedOn = project.CreatedOn,
					Score = project.Score,
				};

				projects.Add(currProject);
			}

			return this.View("All", projects);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Support, Student")]
		public async ASYNC.Task<IActionResult> Upload(int id, IFormFile files)
		{
			var user = this.userManager.GetUserId(this.User);

			try
			{
				if (files != null)
				{
					await this.projectService.Upload(id, user, files);
					TempData["Success"] = "Project file has been uploaded successfully.";
				}
				else
				{
					TempData["Fail"] = "Choose a file to upload.";
				}

				return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Topics", action = "Details", Id = id }));
			}
			catch (DbUpdateException)
			{
				return RedirectToAction("Details", "Topics", new { id = id });
			}
			catch (Exception)
			{
				TempData["Fail"] = "This file extension is not supported.";
				return RedirectToAction("Details", "Topics", new { id = id });
			}
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<IActionResult> Download(int id)
		{
			FileContentResult result = await this.projectService.Download(id);

			if (result == null)
			{
				TempData["Fail"] = "File not found.";
				return RedirectToAction("All", "Projects");
			}

			return result;
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<IActionResult> Edit(int id)
		{
			var project = await this.projectService.GetById(id);
			var topic = await this.topicService.GetById(project.TopicId);
			var student = await this.userService.GetUserById(project.StudentId);

			if (project == null)
			{
				return NotFound();
			}

			var projectToEdit = new ProjectViewModel()
			{
				Id = project.Id,
				TopicName = topic.Title,
				StudentFullName = $"{student.FirstName} {student.LastName}",
				FacultyNumber = student.FacultyNumber,
				FileName = project.FileName,
				CreatedOn = project.CreatedOn,
				Score = project.Score,
			};

			return this.View(projectToEdit);
		}

		[HttpPost, ActionName("Edit")]
		[Authorize(Roles = "Admin, Support, Teacher")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> EditPost(ProjectViewModel model, int id)
		{
			if (ModelState.IsValid)
			{
				await this.projectService.Edit(model, id);
			}

			return RedirectToAction(nameof(All));
		}

		[Authorize(Roles = "Admin, Support, Teacher")]
		public async ASYNC.Task<IActionResult> Delete(int id, bool? saveChangesError = false)
		{
			var project = await this.projectService.GetById(id);
			var topic = await this.topicService.GetById(project.TopicId);
			var student = await this.userService.GetUserById(project.StudentId);

			if (project == null)
			{
				return NotFound();
			}

			var projectToDelete = new ProjectViewModel()
			{
				Id = project.Id,
				TopicName = topic.Title,
				StudentFullName = $"{student.FirstName} {student.LastName}",
				FacultyNumber = student.FacultyNumber,
				FileName = project.FileName,
				CreatedOn = project.CreatedOn,
				Score = project.Score,
			};

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(projectToDelete);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin, Support, Teacher")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				await this.projectService.Delete(id);
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
