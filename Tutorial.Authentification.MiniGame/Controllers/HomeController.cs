using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tutorial.Authentification.MiniGame.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public string Index()
		{
			return "Hello, world!";
		}

		[Authorize]
		[HttpGet]
		public string CheckAuth()
		{
			string aboutUser = "";
			foreach (var claim in HttpContext.User.Claims)
				aboutUser += claim.Type + " " + claim.Value + "\n";
			return "You authorize! Information about you:\n" + aboutUser;
		}
	}
}
