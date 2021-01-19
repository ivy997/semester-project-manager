namespace SemesterProjectManager.Services
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;

	public interface ISubjectService
	{
		public List<Subject> GetAll();

		public int Create(CreateSubjectInputModel input);

		public int GetById();
	}
}
