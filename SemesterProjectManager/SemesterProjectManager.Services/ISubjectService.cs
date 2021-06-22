namespace SemesterProjectManager.Services
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;

	public interface ISubjectService
	{
		public IEnumerable<SubjectServiceModel> GetAll();

		public ASYNC.Task<Subject> GetById(int id);

		public void CreateAsync(CreateSubjectInputModel input);

		public ASYNC.Task<SubjectDetailsServiceModel> Details(int id);

		public void Edit(CreateSubjectInputModel input, int id);

		public ASYNC.Task<SubjectServiceModel> Delete(int id);

		public ASYNC.Task DeleteConfirmed(int id);

		public ASYNC.Task RemoveTeacherFromSubject(ApplicationUser user);

		//public string Test(int id);
	}
}
