namespace SemesterProjectManager.Controllers
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
	using System.IO;
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

		[Authorize(Roles = "Teacher")]
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
		[Authorize(Roles = "Student")]
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
				//return LocalRedirect(new RouteValueDictionary(new { controller = "Topics", action = "Details", Id = id }).ToString());
				return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Topics", action = "Details", Id = id }));
			}
			catch (DbUpdateException)
			{
				return RedirectToAction("Details", "Topics", new { id = id });
			}
		}


		//protected void DownloadFile(object sender, EventArgs e)
		//{
		//	int id = int.Parse((sender as LinkButton).CommandArgument);
		//	byte[] bytes;
		//	string fileName, contentType;
		//	string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
		//	using (SqlConnection con = new SqlConnection(constr))
		//	{
		//		using (SqlCommand cmd = new SqlCommand())
		//		{
		//			cmd.CommandText = "select Name, Data, ContentType from tblFiles where Id=@Id";
		//			cmd.Parameters.AddWithValue("@Id", id);
		//			cmd.Connection = con;
		//			con.Open();
		//			using (SqlDataReader sdr = cmd.ExecuteReader())
		//			{
		//				sdr.Read();
		//				bytes = (byte[])sdr["Data"];
		//				contentType = sdr["ContentType"].ToString();
		//				fileName = sdr["Name"].ToString();
		//			}
		//			con.Close();
		//		}
		//	}
		//	Response.Clear();
		//	Response.Buffer = true;
		//	Response.Charset = "";
		//	Response.Cache.SetCacheability(HttpCacheability.NoCache);
		//	Response.ContentType = contentType;
		//	Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
		//	Response.BinaryWrite(bytes);
		//	Response.Flush();
		//	Response.End();
		//}



		//public async ASYNC.Task<IActionResult> Download(string filename)
		//{
		//	if (filename == null)
		//	{
		//		return Content("filename not present");
		//	}

		//	var path = @"C:\Users\Ivy T\Downloads";

		//	var memory = new MemoryStream();
		//	using (var stream = new FileStream(path, FileMode.Open))
		//	{
		//		await stream.CopyToAsync(memory);
		//	}
		//	memory.Position = 0;

		//	var ext = Path.GetExtension(path).ToLowerInvariant();

		//	return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));
		//}

		[Authorize(Roles = "Teacher")]
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
		[Authorize(Roles = "Teacher")]
		[ValidateAntiForgeryToken]
		public async ASYNC.Task<IActionResult> EditPost(ProjectViewModel model, int id)
		{
			if (ModelState.IsValid)
			{
				await this.projectService.Edit(model, id);
			}

			return RedirectToAction(nameof(All));
		}

		[Authorize(Roles = "Teacher")]
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
		[Authorize(Roles = "Teacher")]
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

		private Dictionary<string, string> GetMimeTypes()
		{
			return new Dictionary<string, string>
			{
				{".pdf", "application/pdf"},
				{".doc", "application/vnd.ms-word"},
				{".docx", "application/vnd.ms-word"},
				{".zip", "application/zip"}
			};
		}
	}
}
