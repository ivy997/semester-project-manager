namespace SemesterProjectManager.Web.ViewModels
{
	using System.Collections.Generic;

	public class SubjectDetailsViewModel : SubjectViewModel
	{
		public IEnumerable<TopicViewModel> Topics { get; set; }
	}
}
