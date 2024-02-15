using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

List<People> peoples = new() 
{
	new People{Login = "myLogin", Password = "123"},
	new People{Login = "youLogin", Password= "321"},
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = AuthOptions.ISSUER,
		ValidateAudience = true,
		ValidAudience = AuthOptions.AUDIENCE,
		ValidateLifetime = true,
		IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
		ValidateIssuerSigningKey = true
	};
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Map("/hi", [Authorize] () => new { message = "It authorize page!" });

app.MapPost("/Login", (People people) => { 
	People? localPeople = peoples.FirstOrDefault(x => x.Login == people.Login && x.Password == people.Password);
	if (localPeople == null) 
	{
		return Results.BadRequest("You are not registred");
	}

	List<Claim> claims = new() { new Claim(ClaimTypes.Name, localPeople.Login)};
	//�������� jwt-������
	var jwt = new JwtSecurityToken(
		issuer: AuthOptions.ISSUER, //��������
		audience: AuthOptions.AUDIENCE, //�����������
		claims: claims, //������ � ������������
		expires: DateTime.UtcNow.AddDays(1), //����� �������� ������
		signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
		);
	var token = new JwtSecurityTokenHandler().WriteToken(jwt); //����� ������������

	var response = new 
	{
		access_token = token,
		username = localPeople.Login
	};
	return Results.Json(response);
});

app.Run();



class People 
{
	public string Login { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public class AuthOptions
{
	// �������� ������
	public const string ISSUER = "MyAuthServer";
	// ����������� ������
	public const string AUDIENCE = "MyAuthClient";
	// ���� ��� ��������
	const string KEY = "mysupersecret_secretsecretsecretkey!123";
	//����� ����������� ���� ������������ ��� ��������� ������
	public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}