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

	public class SubjectsController : Controller
	{
		private ISubjectService subjectService;

		public SubjectsController(ISubjectService subjectService)
		{
			this.subjectService = subjectService;
		}

		[Authorize]
		public ActionResult<IEnumerable<SubjectViewModel>> All()
		{
			var subjects = this.subjectService.GetAll().Select(x => new SubjectViewModel 
			{ 
				Name = x.Name, 
				TeacherId = x.TeacherId 
			}).ToList();
			
			return this.View("All", subjects);
		}

		public IActionResult Create()
		{
			return this.View();
		}

		public IActionResult CreateConfirm()
		{
			// id = servicec.create(model);

			//return this.Redirect("/subject/id");
			return this.View();
		}
	}
}
