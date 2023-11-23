using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyDuoCards.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//SQLite 
builder.Services.AddDbContext<ApplicationContext>();

//MsSQL Server
var provide = builder.Services.BuildServiceProvider();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseRouting();

app.MapControllerRoute(
	name: "default",
    pattern: "{controller=Roles}/{action=Index}/{id?}");
//pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
