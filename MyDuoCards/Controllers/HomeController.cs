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
        private readonly MsApplicationContext _mssql;

        public HomeController(ILogger<HomeController> logger, MsApplicationContext MScontext, ApplicationContext Litecontext)
        {
            _logger = logger;
            _mssql = MScontext;
            _sqlite = Litecontext;
        }

		public async Task<IActionResult> Index()
		{
			//return View(await _mssql.Users.ToListAsync());
			return View(await _sqlite.Users.ToListAsync());
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
