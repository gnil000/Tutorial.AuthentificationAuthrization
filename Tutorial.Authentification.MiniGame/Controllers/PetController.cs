using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tutorial.Authentification.MiniGame.Models;
using Tutorial.Authentification.MiniGame.Storage;
using static Tutorial.Authentification.MiniGame.Models.Pet;
namespace Tutorial.Authentification.MiniGame.Controllers
{
    [Authorize]
	public class PetController : Controller
	{
		private Pets _pets;

		public PetController(Pets pets) 
		{
			_pets = pets;
		}

		[HttpPost]
		public IActionResult Add([FromBody]PetShipping petShipping)
		{
			var user = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
			if(user == null)
			{
				return BadRequest("Не найдено имя пользователя! Невозможно создать животное.");
			}
			_pets.AddPet(user, petShipping.TypePet, petShipping.Name);
			return Ok("Вы приобрели животное");
		}

		[HttpGet]
		public IActionResult Get()
		{
			var user = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
			if(user == null)
			{
				return BadRequest("Невозможно найти пользователя");
			}
			return Ok(_pets.GetPet(user));
		}

		[HttpPost]
		public IActionResult Feeding()
		{
			var user = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
			if (user == null)
			{
				return BadRequest("Невозможно найти пользователя");
			}
			return Ok(_pets.Feeding(user));
		}
	}
}