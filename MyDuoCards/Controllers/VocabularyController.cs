using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;
using MyDuoCards.Models.ViewModels;
using System.Diagnostics;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyDuoCards.Controllers
{
	[Authorize]
    public class VocabularyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public VocabularyController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


		public async Task<IActionResult> Index(string? searchString, int page = 1)
        {
			ViewData["searchString"] = searchString;
			ViewData["page"] = page;

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
				else if (LanguageValidator.IsEnglish(searchString))
				{
					modelRu
						.Where(ruWord => ruWord.RuWriting.Contains(searchString));

				}

			}

			var model = await modelRu
				.Skip((page - 1) * 21)
				.Take(21)
				.ToListAsync();

			return View(model);
        }

		public async Task<IActionResult> AddTheDictionary(int id)
		{
			var ruWord = await _context.RuWords.FindAsync(id);

			var user = await _context.Users
				.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

			Random rand = new Random();
			_context.Dictionaries.Add(new Dictionary()
			{
				UserId = user.Id,
				EnWordId = ruWord.EnWordId,
				DictionaryStatementId = rand.Next(_context.DictionaryStatements.Count()) + 1
			});
			await _context.SaveChangesAsync();

			var previousPageUrl = Request.Headers["Referer"].ToString();
			return Redirect(previousPageUrl);
		}


		public async Task<IActionResult> RemoveDictionary(int id)
		{
			var ruWord = _context.RuWords
				.Include(ruWord => ruWord.EnWord)
				.Where(w => w.Id == id)
				.SingleOrDefault();

			var user = await _context.Users
				.Include(u => u.Dictionaries!)
					.ThenInclude(d => d.EuWord)
				.SingleOrDefaultAsync(u => u.Login == User.Identity!.Name);

			var dictToRemove = user.Dictionaries!.Where(dict => dict.EuWord!.Id == ruWord!.EnWord!.Id).SingleOrDefault();

			_context.Dictionaries.Remove(dictToRemove);

			await _context.SaveChangesAsync();

			var previousPageUrl = Request.Headers["Referer"].ToString();
			return Redirect(previousPageUrl);
		}
	}
}
