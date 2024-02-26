using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { //����� ��������� ������������ ������
	options.TokenValidationParameters = new TokenValidationParameters {
		// ���������, ����� �� �������������� �������� ��� ��������� ������
		ValidateIssuer = true,
		// ������, �������������� ��������
		ValidIssuer = AuthOptions.ISSUER,
		// ����� �� �������������� ����������� ������
		ValidateAudience = true,
		// ��������� ����������� ������
		ValidAudience = AuthOptions.AUDIENCE,
		// ����� �� �������������� ����� �������������
		ValidateLifetime = true,
		// ��������� ����� ������������
		IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
		// ��������� ����� ������������
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
	//�������� jwt-������
	var jwt = new JwtSecurityToken(
		issuer: AuthOptions.ISSUER, //��������
		audience: AuthOptions.AUDIENCE, //�����������
		claims: claims, //������ � ������������
		expires: DateTime.UtcNow.AddDays(1), //����� �������� ������
		signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
		);
	return new JwtSecurityTokenHandler().WriteToken(jwt); //����� ������������ � ������������ �������
});

app.Run();


public class AuthOptions
{
	// �������� ������
	public const string ISSUER = "MyAuthServer";
	// ����������� ������
	public const string AUDIENCE = "MyAuthClient";
	//���� ��� ��������
	const string KEY = "mysupersecret_secretsecretsecretkey!123";
	//����� ����������� ���� ������������ ��� ��������� ������
	public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}