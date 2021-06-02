using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SemesterProjectManager.Data.Models;

namespace SemesterProjectManager.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Subject> Subjects { get; set; }

		public DbSet<Topic> Topics { get; set; }

		public DbSet<Task> Tasks { get; set; }

		public DbSet<Project> Projects { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Subject>(entity =>
			{
				entity.HasMany(s => s.Topics)
					  .WithOne(tp => tp.Subject)
					  .HasForeignKey(tp => tp.SubjectId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.Property<string>("TeacherIdFK");

				entity.HasOne(s => s.Teacher)
					  .WithMany(t => t.Subjects)
					  .HasForeignKey("TeacherIdFK");
			});

			builder.Entity<ApplicationUser>(entity =>
			{
				entity.Property<string>("StudentIdFK");

				entity.HasOne(s => s.Project)
					  .WithOne(p => p.Student)
					  .HasForeignKey<Project>("StudentIdFK")
					  .OnDelete(DeleteBehavior.Restrict);
			});

			builder.Entity<Topic>(entity =>
			{
				entity.HasMany(tp => tp.Tasks)
					  .WithOne(t => t.Topic)
					  .HasForeignKey(t => t.TopicId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(t => t.Student)
					  .WithOne(au => au.Topic)
					  .HasForeignKey<ApplicationUser>(au => au.TopicId)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			builder.Entity<Task>(entity =>
			{
				entity.HasOne(t => t.Student)
					  .WithOne(s => s.Task)
					  .HasForeignKey<ApplicationUser>(s => s.TaskId)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			builder.Entity<Project>(entity =>
			{
				entity.HasOne(p => p.Topic)
					  .WithOne(t => t.Project)
					  .HasForeignKey<Topic>(t => t.ProjectId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
