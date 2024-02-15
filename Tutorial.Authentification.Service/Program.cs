using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//подключена схема аутентификации и сама аутентификация с помощью jwt-токена
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
//подключение сервисов авторизации
builder.Services.AddAuthorization();

var app = builder.Build();

//добавлена middleware аутентификация
app.UseAuthentication();
//добавление middleware авторизации
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/hi", [Authorize]() => "It authorization page!");

app.Run();
