using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tutorial.Authentification.MiniGame.Models;
using Tutorial.Authentification.MiniGame.Storage;

namespace Tutorial.Authentification.MiniGame.Controllers
{
    public class AccountController : Controller
	{
		private Accounts _accounts;

		public AccountController(Accounts accounts) 
		{
			_accounts = accounts;
		}

		[HttpPost]
		public async Task<IActionResult> Registration([FromBody]Account account)
		{
			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, account.Login),
			};

			await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));

			if (_accounts.AddAccount(account))
				return Ok("Вы зарегестрированны!");
			else return BadRequest("Невозможно зарегестрировать пользователя!");
		}
	}
}
