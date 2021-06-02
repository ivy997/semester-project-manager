namespace SemesterProjectManager.Services
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	using Microsoft.EntityFrameworkCore;

	public class SubjectService : ISubjectService
	{
		private readonly ApplicationDbContext context;

		public SubjectService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public void CreateAsync(CreateSubjectInputModel input)
		{
			// Fix error returning
			// Find out why SaveChangesAync doesn't work
			try
			{
				var subject = new Subject()
				{
					Name = input.Name,
					TeacherId = input.TeacherId,
				};

				this.context.Subjects.Add(subject);
				this.context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public async ASYNC.Task<Subject> GetById(int id)
		{
			// Add error handling for null
			// var subject = await this.context.Subjects.FindAsync(id);
			var subject = await this.context.Subjects
							.AsNoTracking()
							.FirstOrDefaultAsync(s => s.Id == id);

			return subject;
		}

		public IEnumerable<SubjectServiceModel> GetAll()
		{
			var subjects = this.context.Subjects.Select(x => new SubjectServiceModel 
			{ 
				Id = x.Id,
				Name = x.Name, 
				TeacherId = x.TeacherId,
			});

			return subjects;
		}

		public async ASYNC.Task<SubjectDetailsServiceModel> Details(int id)
		{
			var subject = await this.GetById(id);

			var model = new SubjectDetailsServiceModel()
			{
				Id = subject.Id,
				Name = subject.Name,
				TeacherId = subject.TeacherId,
			};

			return model;
		}

		public void Edit(CreateSubjectInputModel input, int id)
		{
			// Try to make it async
			var subjectToUpdate = this.GetById(id).Result;

			//Fix error handling
			try
			{
				subjectToUpdate.Name = input.Name;
				subjectToUpdate.TeacherId = input.TeacherId;
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
			}

			this.context.Update(subjectToUpdate);
			this.context.SaveChanges();
		}

		public async ASYNC.Task<SubjectServiceModel> Delete(int id)
		{
			var subject = await this.GetById(id);

			var subjectToDelete = new SubjectServiceModel()
			{
				Id = subject.Id,
				Name = subject.Name,
				TeacherId = subject.TeacherId,
			};

			return subjectToDelete;
		}

		public async ASYNC.Task DeleteConfirmed(int id)
		{
			var subject = await this.GetById(id);

			this.context.Subjects.Remove(subject);
			this.context.SaveChanges();
		}
	}
}
