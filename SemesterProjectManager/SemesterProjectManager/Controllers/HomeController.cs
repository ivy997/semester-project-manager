namespace SemesterProjectManager.Controllers
{
	using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using SemesterProjectManager.Models;
	using SemesterProjectManager.Services;
	using System.Diagnostics;
	using ASYNC = System.Threading.Tasks;

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IEmailSender emailSender;

		public HomeController(ILogger<HomeController> logger,
			IEmailSender emailSender)
		{
			_logger = logger;
			this.emailSender = emailSender;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult SendEmail()
		{
			this.emailSender.SendEmailAsync("", "", "");

			return this.RedirectToAction(nameof(Index));
		}
	}
}
