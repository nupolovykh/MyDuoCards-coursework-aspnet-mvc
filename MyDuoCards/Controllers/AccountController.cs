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
		private readonly ApplicationContext _context;

		public AccountController(ILogger<HomeController> logger, ApplicationContext context)
		{
			_logger = logger;
			_context = context;
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
			if (_context.Users.Where(u => u.Login == regUser.Login || u.Login == regUser.Login || u.Email == regUser.Login || u.Email == regUser.Email).Any())
			{
				ModelState.AddModelError("isRegFailed", "Login or Email already taken");
				return View(regUser);
			}

			var user = new User();
			user.Login = regUser.Login;
			user.Email = regUser.Email;
			user.Password = regUser.Password.ToHash();
            user.RoleId = 2;


            _context.Users.Add(user);
			_context.SaveChangesAsync().Wait();

			return RedirectToAction(nameof(Login));
		}

		// GET: Account/Login
		public IActionResult Login(string? ReturnUrl)
		{
			ViewData["ReturnUrl"] = ReturnUrl;
			return View();
		}

		// POST: Account/Login
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginUser, string? ReturnUrl, bool failed = false) //why he add this "failed"?
		{
			var userToLogin = await _context.Users
				.Where(u =>
				u.Login == loginUser.LoginOrEmail ||
				u.Email == loginUser.LoginOrEmail)
				.Include(u => u.Role)
				.SingleOrDefaultAsync();

			if (userToLogin is null)
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Bad login or email");
				return View(loginUser);
			}
			if (userToLogin?.Password != loginUser.Password.ToHash())
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Bad password");
				return View(loginUser);
			}

            await HttpContext.SignInAsync(userToLogin.ClaimCreator());

			//user.Attandances!.Add(new Attandance() { Time = DateTime.UtcNow });
			////_context.Attandances.Add((new Attandance { UserId = user.Id, Time = DateTime.Now}));
			//await _context.SaveChangesAsync();

			//return Redirect(ReturnUrl);

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
			return RedirectToAction(nameof(Login), "Account");
		}
	}
}
