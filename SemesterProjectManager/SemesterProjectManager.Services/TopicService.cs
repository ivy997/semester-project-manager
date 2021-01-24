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

		public TopicService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public void CreateAsync(CreateTopicInputModel input)
		{
			// Async methods don't save object to db

			var topic = new Topic()
			{
				Title = input.Title,
				Description = input.Description,
				SubjectId = input.SubjectId,
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
	}
}
