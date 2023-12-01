using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using NuGet.Protocol.Plugins;
using SQLitePCL;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication;
using MyDuoCards.Models.Extensions;

namespace MyDuoCards.Controllers
{
	[Authorize]
	public class OptionsController : Controller
	{
		private readonly ApplicationContext _context;

		public OptionsController(ApplicationContext context) { 
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var user = _context.Users.Where(u => u.Login == User.Identity.Name)
                .Include(u => u.Role)
                .SingleOrDefaultAsync();

            return View(await user);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Login,Email,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = user.Password.ToHash(); //need to fix such as usercontroller also
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    var reathirization = await _context.Users
                    .Where(u => u.Login == user.Login)
                    .Include(u => u.Role)
                    .SingleOrDefaultAsync();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, reathirization.Login),
                        new Claim(ClaimTypes.Role, reathirization.Role.Name)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
