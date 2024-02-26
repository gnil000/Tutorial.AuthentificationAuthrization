using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { //метод добавляет конфигурацию токена
	options.TokenValidationParameters = new TokenValidationParameters {
		// указывает, будет ли валидироваться издатель при валидации токена
		ValidateIssuer = true,
		// строка, представляющая издателя
		ValidIssuer = AuthOptions.ISSUER,
		// будет ли валидироваться потребитель токена
		ValidateAudience = true,
		// установка потребителя токена
		ValidAudience = AuthOptions.AUDIENCE,
		// будет ли валидироваться время существования
		ValidateLifetime = true,
		// установка ключа безопасности
		IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
		// валидация ключа безопасности
		ValidateIssuerSigningKey = true,
	};
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", () => "Hello World!");

app.Map("/hi", [Authorize] () => new { message = "It authorize page!"});

app.Map("/Login/{username}", (string username) => {
	var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
	//создание jwt-токена
	var jwt = new JwtSecurityToken(
		issuer: AuthOptions.ISSUER, //издатель
		audience: AuthOptions.AUDIENCE, //потребитель
		claims: claims, //данные о пользователе
		expires: DateTime.UtcNow.AddDays(1), //время действия токена
		signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
		);
	return new JwtSecurityTokenHandler().WriteToken(jwt); //токен записывается и отправляется клиенту
});

app.Run();


public class AuthOptions
{
	// издатель токена
	public const string ISSUER = "MyAuthServer";
	// потребитель токена
	public const string AUDIENCE = "MyAuthClient";
	//ключ для шифрации
	const string KEY = "mysupersecret_secretsecretsecretkey!123";
	//метод возращающий ключ безопасности для генерации токена
	public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}