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
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

		public async Task<IActionResult> Index()
		{
			var user = await _context.Users
                .Include(usr => usr.Attandances)
                .SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

			if (user != null)
			{
                user.Attandances.Add(new Attandance() { Time = DateTime.UtcNow });
                //_context.Attandances.Add((new Attandance { UserId = user.Id, Time = DateTime.Now}));
                await _context.SaveChangesAsync();
			}

			return View(await _context.RuWords.Include(w => w.EnWord).ToListAsync());
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
