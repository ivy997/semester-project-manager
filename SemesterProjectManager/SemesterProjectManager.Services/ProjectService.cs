namespace SemesterProjectManager.Services
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Data.Models.Enums;
	using SemesterProjectManager.Web.ViewModels;
	using SendGrid;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using ASYNC = System.Threading.Tasks;

	public class ProjectService : IProjectService
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<ApplicationUser> userManager;

		public ProjectService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
		{
			this.context = context;
            this.userManager = userManager;
		}

        public async ASYNC.Task<Project> GetById(int id)
        {
            var project = await this.context.Projects
                            .AsNoTracking()
                            .FirstOrDefaultAsync(s => s.Id == id);

            return project;
        }

        public async ASYNC.Task<Project> GetByStudentId(string studentId)
        {
            var project = await this.context.Projects
                            .AsNoTracking()
                            .FirstOrDefaultAsync(s => s.StudentId == studentId);

            return project;
        }

        public async ASYNC.Task<IEnumerable<Project>> GetAll()
		{
            var projects = await this.context.Projects.ToListAsync();

            return projects;
        }

        public async ASYNC.Task Upload(int id, string studentId, IFormFile files)
		{
            if (files.Length > 0)
            {
                var file = Path.GetFileName(files.FileName).Split('.');
                // Getting FileName
                var fileName = file[0];
                // Getting file Extension
                var fileExtension = file[1];
                // concatenating  FileName + FileExtension
                var newFileName = String.Concat(fileName, ".", fileExtension);

                var project = new Project()
                {
                    TopicId = id,
                    StudentId = studentId,
                    FileName = newFileName,
                    FileType = fileExtension,
                };

                using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    project.ProjectFile = target.ToArray();
                }

                this.context.Projects.Add(project);
                this.context.SaveChanges();
            }
        }

        public async ASYNC.Task Edit(ProjectViewModel model, int id)
		{
            var projectToUpdate = await this.GetById(id);

            // Fix error handling (but in controller)
            // Think about asynchronously updating the database

            projectToUpdate.Score = model.Score;

            this.context.Projects.Update(projectToUpdate);
            this.context.SaveChanges();
        }

        public async ASYNC.Task Delete(int id)
        {
            var project = await this.GetById(id);

            this.context.Projects.Remove(project);
            this.context.SaveChanges();
        }
    }
}
