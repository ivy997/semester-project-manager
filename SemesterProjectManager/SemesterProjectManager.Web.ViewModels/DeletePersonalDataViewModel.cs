using System.ComponentModel.DataAnnotations;

namespace SemesterProjectManager.Web.ViewModels
{
	public class DeletePersonalDataViewModel
	{
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RequirePassword { get; set; }
    }
}
