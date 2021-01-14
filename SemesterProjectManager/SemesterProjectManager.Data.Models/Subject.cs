namespace SemesterProjectManager.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Subject : BaseModel<int>
	{
		[Required]
		public string Name { get; set; }

		public int TeacherId { get; set; }

		[Required]
		public Teacher Teacher { get; set; }

		public ICollection<Topic> Topics { get; set; }
	}
}
