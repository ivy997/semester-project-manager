namespace SemesterProjectManager.Services
{
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class SubjectService : ISubjectService
	{
		private readonly ApplicationDbContext context;

		public SubjectService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public int Create(CreateSubjectInputModel input)
		{
			throw new NotImplementedException();
		}

		public int GetById()
		{
			throw new NotImplementedException();
		}

		public List<Subject> GetAll()
		{
			var subjects = context.Subjects.Select(x => new Subject 
			{ 
				Name = x.Name, 
				TeacherId = x.TeacherId 
			}).ToList();

			return subjects;
		}
	}
}
