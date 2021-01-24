namespace SemesterProjectManager.Web.ViewModels
{
	using System;
	using System.Text;
	using System.Collections.Generic;

	using SemesterProjectManager.Data.Models;
	using System.ComponentModel.DataAnnotations;

	public class SubjectViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Subject")]
		public string Name { get; set; }

		[Display(Name = "Teacher")]
		public string TeacherFullName { get; set; }
		//{
		//	get
		//	{
		//		return $"{this.Teacher.Title} {this.Teacher.FirstName} {this.Teacher.LastName}";
		//	}
		//	set
		//	{
		//		teacherFullName = value;
		//	}
		//}
	}
}
