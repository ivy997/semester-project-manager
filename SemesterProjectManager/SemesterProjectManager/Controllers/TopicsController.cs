namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Services;
	using SemesterProjectManager.Web.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class TopicsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly ITopicService topicService;

		public TopicsController(ApplicationDbContext context,
						ITopicService topicService)
		{
			this.context = context;
			this.topicService = topicService;
		}

		//public ActionResult<IEnumerable<TopicViewModel>> All(int subjectId)
		//{
		//	var topics = this.topicService.GetAllBySubjectId(subjectId).Result.Select(x => new TopicViewModel()
		//	{
		//		Name = x.Name,
		//	});

		//	this.ViewData["TopicsList"] = topics;
		//	return this.RedirectToAction(nameof(SubjectsController.Details), "Subjects", subjectId);
		//}

		public IActionResult Create(int subjectId)
		{
			var topicModel = new CreateTopicInputModel();
			topicModel.SubjectId = subjectId;
			return this.View("Create", topicModel);
		}

		[HttpPost]
		public IActionResult Create(CreateTopicInputModel model)
		{
			if (ModelState.IsValid)
			{
				this.topicService.CreateAsync(model);
			}

			return this.Redirect("/Subjects/All");
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

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
			};

			return View(topicToEdit);
		}

		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(EditTopicViewModel input, int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			this.topicService.Edit(input, id);

			return RedirectToAction("All", "Subjects");
		}
	}
}
