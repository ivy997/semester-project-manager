namespace SemesterProjectManager.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SemesterProjectManager.Data;
    using SemesterProjectManager.Data.Models;
    using SemesterProjectManager.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ASYNC = System.Threading.Tasks;

    public class ProjectService : IProjectService
	{
		private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        private readonly ITopicService topicService;

        public ProjectService(ApplicationDbContext context,
            IUserService userService,
            ITopicService topicService)
		{
			this.context = context;
            this.userService = userService;
            this.topicService = topicService;
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
                var fileName = file[0];
                var fileExtension = file[1];

                if (!GetMimeTypes().ContainsKey(fileExtension))
				{
                    throw new Exception("Missing file extention");
                }

                var project = new Project()
                {
                    TopicId = id,
                    StudentId = studentId,
                    FileName = fileName,
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

        public async ASYNC.Task<FileContentResult> Download(int projectId)
		{
            Project project = await this.GetById(projectId);
            ApplicationUser student = await this.userService.GetUserById(project.StudentId);
            Topic topic = await this.topicService.GetById(project.TopicId);

            if (project.ProjectFile != null)
			{
                byte[] file = project.ProjectFile;
                string mimeType = GetMimeTypes()[project.FileType];
                var result = new FileContentResult(file, mimeType);
                result.FileDownloadName = $"{topic.Title} - " +
									   $"{student.FirstName}_{student.LastName}_{student.FacultyNumber} - " +
                                       $"{project.CreatedOn}.{project.FileType}";

                return result;
            }

            return null;
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

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {"pdf", "application/pdf"},
                {"doc", "application/vnd.ms-word"},
                {"docx", "application/vnd.ms-word"},
                {"zip", "application/zip"}
            };
        }
    }
}
