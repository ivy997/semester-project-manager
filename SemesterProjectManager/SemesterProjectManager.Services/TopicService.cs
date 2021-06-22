namespace SemesterProjectManager.Services
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using ASYNC = System.Threading.Tasks;

	using Microsoft.EntityFrameworkCore;

	using SemesterProjectManager.Data;
	using SemesterProjectManager.Data.Models;
	using SemesterProjectManager.Web.ViewModels;
	
	public class TopicService : ITopicService
	{
		private readonly ApplicationDbContext context;
		private readonly ISubjectService subjectService;
		private readonly IUserService userService;

		public TopicService(ApplicationDbContext context, ISubjectService subjectService,
			 IUserService userService)
		{
			this.context = context;
			this.subjectService = subjectService;
			this.userService = userService;
		}

		public IEnumerable<TopicViewModel> GetAll()
		{
			var topics = this.context.Topics.Select(x => new TopicViewModel
			{
				Id = x.Id,
				Name = x.Title,
				SubjectId = x.SubjectId,
				SubjectName = this.subjectService.GetById(x.SubjectId).Result.Name,
			});

			return topics;
		}

		public void CreateAsync(CreateTopicInputModel input)
		{
			// Async methods don't save object to db

			var topic = new Topic()
			{
				Title = input.Title,
				Description = input.Description,
				SubjectId = input.SubjectId,
				Requirements = input.Requirements,
			};

			this.context.Topics.Add(topic);
			this.context.SaveChanges();
		}

		public async ASYNC.Task<IEnumerable<TopicViewModel>> GetAllBySubjectId(int subjectId)
		{
			var topics = await this.context.Topics
				.Where(x => x.SubjectId == subjectId)
				.Select(t => new TopicViewModel()
			{
				Id = t.Id,
				Name = t.Title,
			})
			.ToListAsync();

			return topics;
		}

		public async ASYNC.Task<Topic> GetById(int id)
		{
			var topic = await this.context.Topics
							.AsNoTracking()
							.FirstOrDefaultAsync(s => s.Id == id);

			return topic;
		}

		public void Edit(EditTopicViewModel input, int id)
		{
			// Try to make it async
			var topicToUpdate = this.GetById(id).Result;

			// Fix error handling (but in controller)
			// Think about asynchronously updating the database
			
			topicToUpdate.Title = input.Title;
			topicToUpdate.Description = input.Description;
			topicToUpdate.StateOfTopic = input.StateOfApproval;

			this.context.Topics.Update(topicToUpdate);
			this.context.SaveChanges();
		}

		public async ASYNC.Task Delete(int id)
		{
			var topic = await this.GetById(id);

			var student = await this.userService.GetUserById(topic.StudentId);
			if (student != null)
			{
				student.TaskId = null;
				student.TopicId = null;
				student.ProjectId = null;
				this.context.Users.Update(student);
			}

			this.context.Topics.Remove(topic);
			this.context.SaveChanges();
		}
	}
}
