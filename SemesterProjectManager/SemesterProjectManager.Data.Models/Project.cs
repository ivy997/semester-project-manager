﻿namespace SemesterProjectManager.Data.Models
{
	public class Project : BaseModel<int>
	{
		public int StudentId { get; set; }

		public Student Student { get; set; }

		public Topic Topic { get; set; }

		public byte[] ProjectFile { get; set; }

		public int Score { get; set; }
	}
}
