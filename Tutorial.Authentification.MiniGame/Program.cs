using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Tutorial.Authentification.MiniGame.Storage;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Account/Registration");
builder.Services.AddAuthorization();


builder.Services.AddSingleton<Accounts>();
builder.Services.AddSingleton<Pets>();


builder.Services.AddControllers();



builder.Host.UseSerilog();

//builder.Services.AddCors(options => {
//	options.AddPolicy("MyPolicy", builder => {
//		builder.AllowAnyOrigin();
//		builder.AllowAnyMethod();
//		builder.AllowAnyHeader();
//		builder.AllowCredentials();
//	});
//});


var app = builder.Build();

// global cors policy
app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true) // allow any origin
	.AllowCredentials()); // allow credentials

//app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapDefaultControllerRoute();

app.Run();
