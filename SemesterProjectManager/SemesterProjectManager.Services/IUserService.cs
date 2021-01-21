namespace SemesterProjectManager.Services
{
	using SemesterProjectManager.Data.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IUserService
	{
		public Task<IList<ApplicationUser>> GetAllUsers();

		public Task<int> GetUserById();

		public Task<IList<ApplicationUser>> GetStudents();

		public Task<int> GetStudentById();

		public Task<IEnumerable<ApplicationUser>> GetTeachers();

		public Task<int> GetTeacherById();

		public Task<IEnumerable<string>> GetTeachersFullName();
	}
}
