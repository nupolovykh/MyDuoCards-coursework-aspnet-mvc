using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyDuoCards.Models;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options => options.AreaViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml"));

// SQLite: connection string (and therefore the DB file's location) comes from
// config, defaulting to appsettings.json's ConnectionStrings:Default but
// overridable per environment (e.g. ConnectionStrings__Default env var) without
// touching code. The SQLite provider creates the .db file itself on first use,
// but it won't create a missing containing directory, so that's done explicitly.
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=Database/fiction.db";

var dataSourceDirectory = Path.GetDirectoryName(
    Path.GetFullPath(new SqliteConnectionStringBuilder(connectionString).DataSource));
if (!string.IsNullOrEmpty(dataSourceDirectory))
{
    Directory.CreateDirectory(dataSourceDirectory);
}

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(); //options => options.LoginPath = "/Authorization/Login"
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseRouting();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
