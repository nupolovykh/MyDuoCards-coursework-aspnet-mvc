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

		public async Task<IActionResult> Index(string? searchString = "", int page = 1)
		{
            int quantityOfElements = 13;
            ViewData["searchString"] = searchString;
			ViewData["page"] = page;

			var user = await _context.Users
				.Include(usr => usr.Attandances)
				.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

			{
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
            }	//not so important work


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
				.Skip((page - 1) * quantityOfElements)
				.Take(quantityOfElements);

			var count = modelRuPlus.Where(ruWord => ruWord.EnWord!.Dictionaries!.Any(dict => dict.User!.Login == User.Identity!.Name)).Count();

			List<int> list = null;
			if(count != 0)
			{
				int maxIndex = (count / quantityOfElements) + 1;
				list = ListBuilderForButtons.GetButtonIndexes(page, maxIndex);
			}
			ViewData["list"] = list;

			return View(viewModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}
