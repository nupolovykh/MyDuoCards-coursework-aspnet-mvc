using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.ViewModels;
using System.Diagnostics;

namespace MyDuoCards.Controllers
{
	[Authorize]
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _sqlite;

        public HomeController(ILogger<HomeController> logger, ApplicationContext Litecontext)
        {
            _logger = logger;
            _sqlite = Litecontext;
        }

		public async Task<IActionResult> Index()
		{
			var user = await _sqlite.Users
                .Include(usr => usr.Attandances)
                .SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

			if (user != null)
			{
                user.Attandances.Add(new Attandance() {/* UserId = user.Id, */Time = DateTime.UtcNow });

				//_sqlite.Attandances.Add((new Attandance { UserId = user.Id, Time = DateTime.Now}));
				await _sqlite.SaveChangesAsync();
			}

			return View(await _sqlite.Users.Include(u => u.Role).ToListAsync());
		}

        public IActionResult Options()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
