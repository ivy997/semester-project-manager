namespace SemesterProjectManager.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Subject : BaseModel<int>
	{
		[Required]
		public string Name { get; set; }

		public string TeacherId { get; set; }

		public ApplicationUser Teacher { get; set; }

		public ICollection<Topic> Topics { get; set; }
	}
}
