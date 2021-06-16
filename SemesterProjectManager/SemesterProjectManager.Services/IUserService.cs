namespace SemesterProjectManager.Services
{
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IUserService
	{
		public IEnumerable<ApplicationUser> GetAllUsers();

		public Task<ApplicationUser> GetUserById(string id);

		public Task<IList<ApplicationUser>> GetStudents();

		//public Task<ApplicationUser> GetStudentById(string id);

		public Task<IEnumerable<ApplicationUser>> GetTeachers();

		//public Task<ApplicationUser> GetTeacherById(string id);

		public Task<CreateSubjectInputModel> GetTeachersFullNameWithId();
	}
}
