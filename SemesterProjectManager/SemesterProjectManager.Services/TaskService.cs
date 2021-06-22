namespace SemesterProjectManager.Services
{
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	using System.Collections.Generic;
	using System.Linq;
	using ASYNC = System.Threading.Tasks;

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

		public IEnumerable<TaskServiceModel> GetAll()
		{
			var tasks = this.context.Tasks
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
				CreatedOn = taskModel.CreatedOn.ToUniversalTime(),
				DueDate = taskModel.DueDate,
				TopicId = taskModel.TopicId,
				StudentId = taskModel.StudentId,
			};

			topic.Tasks.Add(task);

			this.context.Add(task);
			this.context.Topics.Update(topic);
			this.context.SaveChanges();

			student.TaskId = task.Id;

			this.context.Users.Update(student);
			this.context.SaveChanges();
		}

		public async ASYNC.Task Edit(EditTaskViewModel model, int id)
		{
			// Try to make it async
			Task taskToUpdate = await this.GetById(id);
			Topic topicToUpdate = await this.topicService.GetById(model.TopicId);
			ApplicationUser student = await this.userService.GetUserById(model.StudentId);

			// Fix error handling (but in controller)
			// Think about asynchronously updating the database

			taskToUpdate.MainTask = model.MainTask;
			taskToUpdate.OutputData = model.OutputData;
			taskToUpdate.DueDate = model.DueDate;
			taskToUpdate.IsApproved = model.IsApproved;

			if (model.IsApproved)
			{
				topicToUpdate.StudentId = model.StudentId;
				student.TopicId = model.TopicId;
			}

			this.context.Tasks.Update(taskToUpdate);
			this.context.Topics.Update(topicToUpdate);
			this.context.Users.Update(student);
			this.context.SaveChanges();
		}

		public async ASYNC.Task Delete(int id)
		{
			// Threading error
			Task task = this.GetById(id).Result;
			Topic topicToUpdate = this.topicService.GetById(task.TopicId).Result;
			ApplicationUser student = await this.userService.GetUserById(task.StudentId);

			topicToUpdate.StudentId = null;
			student.TaskId = null;

			this.context.Tasks.Remove(task);
			this.context.Topics.Update(topicToUpdate);
			this.context.Users.Update(student);
			this.context.SaveChanges();
		}
	}
}
