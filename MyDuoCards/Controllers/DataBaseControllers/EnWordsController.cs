using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;

namespace MyDuoCards.Controllers.DataBaseControllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class EnWordsController : Controller
	{
		private readonly ApplicationContext _context;
        private int quantityOfElements = Constants.AmountOfLanguageTableElements;

        public EnWordsController(ApplicationContext context)
		{
			_context = context;
		}

		// GET: EnWords
		public async Task<IActionResult> Index(string? searchString, int page = 1)
		{
			ViewData["page"] = page;
			ViewData["searchString"] = searchString;

			var words = _context.EnWords;

            var modelEnPlus = new List<EnWord>();
            if (!String.IsNullOrEmpty(searchString))
			{
                modelEnPlus = await words.Where(w => w.EnWriting.Contains(searchString)).ToListAsync();
            }
			else
			{
                modelEnPlus = await words.ToListAsync();
            }

            var applicationContext = modelEnPlus
                .Skip((page - 1) * quantityOfElements)
				.Take(quantityOfElements);

            var count = modelEnPlus.Count();

            List<int> list = null;
            if (count != 0)
            {
                int maxIndex = (count / quantityOfElements);
				if (count % quantityOfElements != 0) maxIndex++;

                list = ListBuilderForButtons.GetButtonIndexes(page, maxIndex);
            }
            ViewData["list"] = list;

            return _context.EnWords != null ? 
				View(applicationContext) :
				Problem("Entity set 'ApplicationContext.EnWords' is null.");
		}

		// GET: EnWords/Details/5
		public async Task<IActionResult> Details(int id, string? searchString, int page = 1)
		{
            ViewData["page"] = page;
            ViewData["searchString"] = searchString;

            if (id == null || _context.EnWords == null)
			{
				return NotFound();
			}

			var enWord = await _context.EnWords
				.FirstOrDefaultAsync(m => m.Id == id);
			if (enWord == null)
			{
				return NotFound();
			}

			return View(enWord);
		}

		// GET: EnWords/Create
		public IActionResult Create(int? page)
		{
			ViewData["page"] = page;
			return View();
		}

		// POST: EnWords/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,EnWriting")] EnWord enWord, int page = 1)
		{
            if (ModelState.IsValid)
			{
				_context.Add(enWord);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index), new { page = page });
			}
			return View(enWord);
		}

		// GET: EnWords/Edit/5
		public async Task<IActionResult> Edit(string? searchString, int? id, int? page = 1)
		{
            ViewData["page"] = page;
            ViewData["searchString"] = searchString;

            if (id == null || _context.EnWords == null)
			{
				return NotFound();
			}

			var enWord = await _context.EnWords.FindAsync(id);
			if (enWord == null)
			{
				return NotFound();
			}
			return View(enWord);
		}

		// POST: EnWords/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string? searchString, int id, [Bind("Id,EnWriting")] EnWord enWord, int? page)
		{
			if (id != enWord.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(enWord);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EnWordExists(enWord.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

                return RedirectToAction(nameof(Index), new { page = page.ToString(), searchString = searchString});
			}
			return View(enWord);
		}

		// GET: EnWords/Delete/5
		public async Task<IActionResult> Delete(int? id, string? searchString, int page = 1)
		{
			ViewData["page"] = page;
            ViewData["searchString"] = searchString;

            if (id == null || _context.EnWords == null)
			{
				return NotFound();
			}

			var enWord = await _context.EnWords
				.FirstOrDefaultAsync(m => m.Id == id);
			if (enWord == null)
			{
				return NotFound();
			}

			return View(enWord);
		}

		// POST: EnWords/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id, string? searchString, int page = 1)
		{
			if (_context.EnWords == null)
			{
				return Problem("Entity set 'ApplicationContext.EnWords'  is null.");
			}
			var enWord = await _context.EnWords.FindAsync(id);
			if (enWord != null)
			{
				_context.EnWords.Remove(enWord);
			}
			
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index), new { page = page.ToString(), searchString = searchString });
		}

		private bool EnWordExists(int id)
		{
		  return (_context.EnWords?.Any(e => e.Id == id)).GetValueOrDefault();
		}
    }
}
