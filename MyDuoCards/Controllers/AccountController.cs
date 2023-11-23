using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.ViewModels;
using MyDuoCards.Models;
using MyDuoCards.Models.Extensions;
using Microsoft.Extensions.Logging;

namespace MyDuoCards.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationContext _sqlite;

		public AccountController(ILogger<HomeController> logger, ApplicationContext Litecontext)
		{
			_logger = logger;
			_sqlite = Litecontext;
		}

		// GET: Account/Register
		public IActionResult Register()
		{
			return View();
		}

		// POST: Account/Register
		[HttpPost]
		public IActionResult Register(RegisterModel regUser)
		{
			if (!ModelState.IsValid || regUser.Password != regUser.ConfirmPassword)
			{
				ModelState.AddModelError("isRegFailed", "Passwords incorrect");
				return View(regUser);
			}
			if (_sqlite.Users.Where(u => u.UserLogin == regUser.Login || u.UserLogin == regUser.Login || u.UserEmail == regUser.Login || u.UserEmail == regUser.Email).Any())
			{
				ModelState.AddModelError("isRegFailed", "Login or Email already taken");
				return View(regUser);
			}

			var user = new User();
			user.UserLogin = regUser.Login;
			user.UserEmail = regUser.Email;
			user.UserPassword = regUser.Password.ToHash();

			_sqlite.Users.Add(user);
			_sqlite.SaveChangesAsync().Wait();

			return RedirectToAction(nameof(Login));
		}

		// GET: Account/Login
		public IActionResult Login()
		{
			return View();
		}

		// POST: Account/Login
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginUser, bool failed = false)
		{

			var userToLogin = await _sqlite.Users.Where(u => u.UserLogin == loginUser.LoginOrEmail || u.UserEmail == loginUser.LoginOrEmail).SingleOrDefaultAsync();
			//_logger.LogInformation(userToLogin.ToString());
			if (userToLogin is null)
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Bad login or email");
				return View(loginUser);
			}
			if (userToLogin?.UserPassword != loginUser.Password.ToHash())
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Bad password");
				return View(loginUser);
			}

			Authenticate(loginUser.LoginOrEmail);
			return RedirectToAction(nameof(Index), "Home");
		}

		private void Authenticate(string userName)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)

			};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)).Wait();
			//_logger.LogError(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		[Authorize]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
			return RedirectToAction(nameof(Login), "Account");
		}
	}
}
