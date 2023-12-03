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

            // var result = user
            // var viewResult = View(result)
            // return viewResult

            return View(userForView);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Login,Email,RoleName,Password,ConfirmPassword,RoleId")] EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users
                    .Where(u => u.Login == User.Identity.Name)
                    .Include(u => u.Role)
                    .SingleOrDefaultAsync();

                    user.Login = model.Login;
                    user.Email = model.Email;

                    if (user.Password != model.Password)
                    {
                        if (model.Password == model.ConfirmPassword)
                        {
                            user.Password = model.Password.ToHash();
                        }
                        else
                        {
                            ModelState.AddModelError("EdittingError", "Changing password failed");
                            return View(model);
                        }
                    }

                    _context.Update(user);

                    await _context.SaveChangesAsync();

                    await HttpContext.SignInAsync(user.ClaimCreator());
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
