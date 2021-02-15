namespace SemesterProjectManager.Services
{
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;

	public interface IProjectService
	{
		public ASYNC.Task Upload(int id, string studentId, IFormFile files);

		public ASYNC.Task<Project> GetById(int id);

		public ASYNC.Task<Project> GetByStudentId(string studentId);

		public ASYNC.Task<IEnumerable<Project>> GetAll();

		public ASYNC.Task Edit(ProjectViewModel model, int id);

		public ASYNC.Task Delete(int id);
	}
}
