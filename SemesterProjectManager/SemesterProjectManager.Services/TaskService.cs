namespace SemesterProjectManager.Services
{
	using System.Linq;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using SemesterProjectManager.Web.ViewModels;
	using Microsoft.EntityFrameworkCore;

	public class TaskService : ITaskService
	{
		private readonly ApplicationDbContext context;
		private readonly ITopicService topicService;
		private readonly IUserService userService;

		public TaskService(ApplicationDbContext context,
			ITopicService topicService,
			IUserService userService)
		{
			this.context = context;
			this.topicService = topicService;
			this.userService = userService;
		}

		public async ASYNC.Task<IEnumerable<TaskServiceModel>> GetAllByTopicId(int topicId)
		{
			var tasks = this.context.Tasks
				.Where(x => x.TopicId == topicId)
				.Select(x => new TaskServiceModel()
				{
					Id = x.Id,
					StudentId = x.StudentId,
					TopicId = x.TopicId,
					CreatedOn = x.CreatedOn,
					IsApproved = x.IsApproved,
				});

			return tasks;
		}

		public async ASYNC.Task<Task> GetById(int id)
		{
			var task = await this.context.Tasks
							.AsNoTracking()
							.FirstOrDefaultAsync(s => s.Id == id);

			return task;
		}

		public async ASYNC.Task Create(CreateTaskViewModel taskModel)
		{
			var topic = await this.topicService.GetById(taskModel.TopicId);
			var student = await this.userService.GetUserById(taskModel.StudentId);

			var task = new Task()
			{
				MainTask = taskModel.MainTask,
				OutputData = taskModel.OutputData,
				CreatedOn = taskModel.CreatedOn,
				DueDate = taskModel.DueDate,
				TopicId = taskModel.TopicId,
				StudentId = taskModel.StudentId,
			};

			this.context.Add(task);
			this.context.SaveChanges();
		}

		public async ASYNC.Task Edit(EditTaskViewModel model, int id)
		{
			// Try to make it async
			var taskToUpdate = await this.GetById(id);

			// Fix error handling (but in controller)
			// Think about asynchronously updating the database

			taskToUpdate.MainTask = model.MainTask;
			taskToUpdate.OutputData = model.OutputData;
			taskToUpdate.DueDate = model.DueDate;
			taskToUpdate.IsApproved = model.IsApproved;

			this.context.Tasks.Update(taskToUpdate);
			this.context.SaveChanges();
		}

		public async ASYNC.Task Delete(int id)
		{
			var task = await this.GetById(id);

			this.context.Tasks.Remove(task);
			this.context.SaveChanges();
		}
	}
}
