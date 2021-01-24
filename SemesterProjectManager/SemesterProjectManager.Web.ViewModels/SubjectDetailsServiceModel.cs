namespace SemesterProjectManager.Web.ViewModels
{
	using System.Collections.Generic;

	public class SubjectDetailsServiceModel : SubjectServiceModel
	{
		public IEnumerable<TopicViewModel> Topics { get; set; }
	}
}
