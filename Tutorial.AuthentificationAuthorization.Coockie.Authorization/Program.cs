using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

List<People> peoples = new()
{
	new People{Login = "myLogin", Password = "123"},
	new People{Login = "youLogin", Password= "321"},
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Login");
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Map("/hi", [Authorize] () => new { message = "It authorize page!" });

app.MapPost("/Login", async (People people, HttpContext context) => {
	People? localPeople = peoples.FirstOrDefault(x => x.Login == people.Login && x.Password == people.Password);
	if(localPeople == null)
		return Results.BadRequest("You are not registred!");
	List<Claim> claims = new() { new Claim(ClaimTypes.Name, localPeople.Login)};
	ClaimsIdentity identity = new(claims, "Cookies");
	ClaimsPrincipal principal = new(identity);

	await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);	

	return Results.Ok("set cookie");
});

app.Run();


class People
{
	public string Login { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}
