using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;
using MyDuoCards.Models.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace MyDuoCards.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationContext _context;
        private int amountOfElements = Constants.AmountOfCardsHome;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
		{
			_logger = logger;
			_context = context;
		}

        public async Task<IActionResult> Index(string? searchString = "", int page = 1)
		{
            ViewData["searchString"] = searchString;
			ViewData["page"] = page;

            var modelRu = _context.RuWords
					.Include(ruWord => ruWord.EnWord)
						.ThenInclude(enWord => enWord!.Dictionaries!)
							.ThenInclude(dict => dict.User);


			var modelRuPlus = new List<RuWord>();

            if (!String.IsNullOrEmpty(searchString))
			{
				if (LanguageValidator.IsRussian(searchString))
				{
                    modelRuPlus = await modelRu
                        .Where(ruWord => ruWord.RuWriting.Contains(searchString))
					.ToListAsync();

                }
				else if(searchString.IsEnglish())
				{
                    modelRuPlus = await modelRu
                        .Where(ruWord => ruWord.EnWord!.EnWriting.Contains(searchString))
                    .ToListAsync();
                }

			}
			else modelRuPlus = await modelRu.ToListAsync();

			var viewModel = modelRuPlus.Where(ruWord => ruWord.EnWord!.Dictionaries!.Any(dict => dict.User!.Login == User.Identity!.Name))
				.Skip((page - 1) * amountOfElements)
				.Take(amountOfElements);

			var count = modelRuPlus.Where(ruWord => ruWord.EnWord!.Dictionaries!.Any(dict => dict.User!.Login == User.Identity!.Name)).Count();

			List<int> list = null;
			if(count != 0)
			{
                int maxIndex = (count / amountOfElements);
                if (count % amountOfElements != 0) maxIndex++;
                list = ListBuilderForButtons.GetButtonIndexes(page, maxIndex);
			}
			ViewData["list"] = list;

			return View(viewModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}

        [HttpGet]
        public async Task<JsonResult> AttendanceGET()
        {
			var attendance = await _context.Attendances
				.Include(a => a.User)
				.Where(a => a.User.Login == User.Identity!.Name).ToListAsync();

            if (attendance == null)
            {
                return Json(0);
            }

            return Json(attendance.Count);
        }

        [HttpPost]
        public async void AttendancePOST()
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity!.Name);

            if (user != null)
            {
                var lastAttendance = await _context.Attendances
                .Include(a => a.User)
                .Where(a => a.User.Login == User.Identity.Name)
                .OrderByDescending(a => a.Time)
                .FirstOrDefaultAsync();

                DateTime subtractedDate = DateTime.UtcNow.AddDays(-(Constants.TimeIntervalForAttendances));

                if (lastAttendance == null || lastAttendance.Time < subtractedDate)
                {
                    _context.Attendances.Add((new Attendance { UserId = user.Id, Time = DateTime.UtcNow }));
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}