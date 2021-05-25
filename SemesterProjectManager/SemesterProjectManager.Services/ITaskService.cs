namespace SemesterProjectManager.Services
{
	using ASYNC = System.Threading.Tasks;

	using SemesterProjectManager.Web.ViewModels;
	using System.Collections.Generic;
	using SemesterProjectManager.Data.Models;

	public interface ITaskService
	{
		public IEnumerable<TaskServiceModel> GetAll();

		public ASYNC.Task Create(CreateTaskViewModel taskModel);

		public ASYNC.Task<IEnumerable<TaskServiceModel>> GetAllByTopicId(int topicId);

		public ASYNC.Task<Task> GetById(int id);

		public ASYNC.Task Edit(EditTaskViewModel model, int id);

		public ASYNC.Task Delete(int id);
	}
}
