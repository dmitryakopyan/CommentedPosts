using System.Diagnostics;
using CommentedPostsUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPostsUI.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return Redirect("Posts");
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

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
	}
}
