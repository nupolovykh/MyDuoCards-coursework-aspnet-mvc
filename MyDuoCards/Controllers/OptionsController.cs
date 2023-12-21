using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;
using MyDuoCards.Models.ViewModels;

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
			var user = await _context.Users.Where(u => u.Login == User.Identity.Name)
                .Include(u => u.Role)
                .SingleOrDefaultAsync();

            EditUserModel userForView = new EditUserModel();
            userForView.Login = user.Login;
            userForView.Email = user.Email;
            userForView.RoleName = user.Role.Name;
            userForView.Password = user.Password;
            userForView.RoleId = user.RoleId;

            return View(userForView);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Login,Email,RoleName,Password,ConfirmPassword,RoleId")] EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Where(u => u.Login == User.Identity!.Name)
                    .Include(u => u.Role)
                    .SingleOrDefaultAsync();

                if (user == null)
                {
                    ModelState.AddModelError("EdittingError", "User not found");
                    return View(model);
                }

                if (user.Login != model.Login && await _context.Users.AnyAsync(u => u.Login == model.Login))
                {
                    ModelState.AddModelError("EdittingError", "Login already exists");
                    return View(model);
                }

                if (user.Email != model.Email && await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("EdittingError", "Email already exists");
                    return View(model);
                }

                user.Login = model.Login;
                user.Email = model.Email;

                if (!string.IsNullOrEmpty(model.Password) && model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("EdittingError", "Passwords do not match");
                    return View(model);
                }

                if (!string.IsNullOrEmpty(model.Password) && user.Password != model.Password)
                {
                    user.Password = model.Password.ToHash();
                }

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    await HttpContext.SignInAsync(user.ClaimCreator());

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View(model);
        }

    }
}
