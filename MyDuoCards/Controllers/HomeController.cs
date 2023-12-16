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
using System.Diagnostics;
using System.Linq;

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

		public async Task<IActionResult> Index(string? searchString, int page = 1)
		{

			ViewData["searchString"] = searchString;
			ViewData["page"] = page;

			var user = await _context.Users
				.Include(usr => usr.Attandances)
				.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

			////var previousPageUrl = Request.Headers["Referer"].ToString();

			////foreach (var header in Request.Headers)
			////{
			////	_logger.LogWarning($"{header.Key}  ---   {header.Value}");
			////}

			//if (user != null)		// this logic should be did over !!!!!!!!!!!!!!!!
			//{
			//	user.Attandances!.Add(new Attandance() { Time = DateTime.UtcNow });
			//	//_context.Attandances.Add((new Attandance { UserId = user.Id, Time = DateTime.Now}));
			//	await _context.SaveChangesAsync();
			//}


			var modelRu = _context.RuWords
					.Include(ruWord => ruWord.EnWord)
						.ThenInclude(enWord => enWord.Dictionaries!)
							.ThenInclude(dict => dict.User);


            if (!String.IsNullOrEmpty(searchString))
			{
				if (LanguageValidator.IsRussian(searchString))
				{
					modelRu
						.Where(ruWord => ruWord.RuWriting.Contains(searchString));

                }
				else if(searchString.IsEnglish())
				{
					modelRu
						.Where(ruWord => ruWord.EnWord!.EnWriting.Contains(searchString));
				}

			}

            var model = await modelRu.Where(ruWord => ruWord.EnWord!.Dictionaries!.Any(dict => dict.User!.Login == User.Identity!.Name))
				.Skip((page - 1) * 14)
				.Take(14)
				.ToListAsync();

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}
