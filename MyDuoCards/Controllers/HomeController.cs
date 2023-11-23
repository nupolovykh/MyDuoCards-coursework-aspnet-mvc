using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
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
			return View(await _sqlite.Users.ToListAsync());
		}

		public IActionResult Privacy()
        {
            return View();
        }
    }
}
