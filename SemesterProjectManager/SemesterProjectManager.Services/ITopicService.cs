namespace SemesterProjectManager.Services
{
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;

	public interface ITopicService
	{
		public ASYNC.Task<IEnumerable<TopicViewModel>> GetAllBySubjectId(int subjectId);

		public ASYNC.Task<Topic> GetById(int id);

		public void CreateAsync(CreateTopicInputModel input);

		public void Edit(EditTopicViewModel input, int id);
	}
}
