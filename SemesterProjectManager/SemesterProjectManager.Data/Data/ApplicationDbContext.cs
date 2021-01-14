using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SemesterProjectManager.Data.Models;
using System.Linq;

namespace SemesterProjectManager.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Student> Students { get; set; }

		public DbSet<Teacher> Teachers { get; set; }

		public DbSet<Subject> Subjects { get; set; }

		public DbSet<Topic> Topics { get; set; }

		public DbSet<Task> Tasks { get; set; }

		public DbSet<Project> Projects { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//foreach (var relationship in )
			//{
			//	relationship.DeleteBehavior = DeleteBehavior.Restrict;
			//}

			var foreignKeys = builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

			foreach (var relationship in foreignKeys)
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			base.OnModelCreating(builder);

			builder.Entity<Subject>(entity =>
			{
				entity.HasMany(s => s.Topics)
					  .WithOne(tp => tp.Subject)
					  .HasForeignKey(tp => tp.SubjectId);

				entity.Property<string>("TeacherIdFK");

				entity.HasOne(s => s.Teacher)
					  .WithMany(t => t.Subjects)
					  .HasForeignKey("TeacherIdFK");
			});

			builder.Entity<Student>(entity =>
			{
				//entity.HasOne(s => s.Topic)
				//	  .WithMany(tp => tp.Students)
				//	  .HasForeignKey(s => s.TopicId);

				entity.Property<string>("StudentIdFK");

				entity.HasOne(s => s.Project)
					  .WithOne(p => p.Student)
					  .HasForeignKey<Project>("StudentIdFK")
					  .OnDelete(DeleteBehavior.Restrict);
			});

			builder.Entity<Task>(entity =>
			{
				entity.HasOne(t => t.Student)
					  .WithOne(s => s.Task)
					  .HasForeignKey<Student>(s => s.TaskId);

				entity.HasOne(t => t.Topic)
					  .WithOne(tp => tp.Task)
					  .HasForeignKey<Topic>(tp => tp.TaskId);
			});

			builder.Entity<Project>(entity =>
			{
				entity.HasOne(p => p.Topic)
					  .WithOne(t => t.Project)
					  .HasForeignKey<Topic>(t => t.ProjectId);
			});
		}
	}
}
