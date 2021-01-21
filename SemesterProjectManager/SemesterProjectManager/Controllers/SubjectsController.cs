namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Services;
	using SemesterProjectManager.Web.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

	public class SubjectsController : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly ISubjectService subjectService;
		private readonly IUserService userService;

		public SubjectsController(ISubjectService subjectService,
				IUserService userService)
		{
			this.subjectService = subjectService;
			this.userService = userService;
		}

		[Authorize]
		public ActionResult<IEnumerable<SubjectViewModel>> All()
		{
			var subjects = this.subjectService.GetAll().Select(x => new SubjectViewModel 
			{ 
				Name = x.Name, 
				TeacherId = x.TeacherId 
			})
			.ToList();
			
			return this.View("All", subjects);
		}

		public async Task<IActionResult> Create()
		{
			var createVM = new CreateSubjectInputModel();
			var teachers = await userService.GetTeachersFullName();
			createVM.TeachersList = teachers;
			return this.View("Create", createVM);
		}

		public IActionResult CreateConfirm()
		{
			// id = servicec.create(model);

			//return this.Redirect("/subject/id");
			return this.View();
		}
	}
}
