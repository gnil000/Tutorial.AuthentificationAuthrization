using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//���������� ����� �������������� � ���� �������������� � ������� jwt-������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
//����������� �������� �����������
builder.Services.AddAuthorization();

var app = builder.Build();

//��������� middleware ��������������
app.UseAuthentication();
//���������� middleware �����������
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/hi", [Authorize]() => "It authorization page!");

app.Run();
