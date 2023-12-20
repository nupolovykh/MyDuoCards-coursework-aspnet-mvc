using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;
using NuGet.Packaging.Signing;

namespace MyDuoCards.Controllers.DataBaseControllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RuWordsController : Controller
    {
        private readonly ApplicationContext _context;

        public RuWordsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: RuWords
        public async Task<IActionResult> Index(string? searchString, int page = 1)
        {
            int quantityOfElements = 10;
            ViewData["page"] = page;
            ViewData["searchString"] = searchString;

            var words = _context.RuWords.Include(r => r.EnWord);

            var modelRuPlus = new List<RuWord>();
            if (!String.IsNullOrEmpty(searchString))
            {
                modelRuPlus = await words.Where(w => w.RuWriting.Contains(searchString)).ToListAsync();
            }
            else
            {
                modelRuPlus = await words.ToListAsync();
            }

            var applicationContext = modelRuPlus
                .Skip((page - 1) * quantityOfElements)
                .Take(quantityOfElements);

            var count = modelRuPlus.Count();

            List<int> list = null;
            if (count != 0)
            {
                int maxIndex = (count / quantityOfElements) + 1;
                list = ListBuilderForButtons.GetButtonIndexes(page, maxIndex);
            }
            ViewData["list"] = list;

            return View(applicationContext);
        }

        // GET: RuWords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RuWords == null)
            {
                return NotFound();
            }

            var ruWord = await _context.RuWords
                .Include(r => r.EnWord)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruWord == null)
            {
                return NotFound();
            }

            return View(ruWord);
        }

        // GET: RuWords/Create
        public IActionResult Create()
        {
            ViewData["EnWordId"] = new SelectList(_context.EnWords, "Id", "EnWriting");
            return View();
        }

        // POST: RuWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RuWriting,EnWordId")] RuWord ruWord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ruWord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnWordId"] = new SelectList(_context.EnWords, "Id", "EnWriting", ruWord.EnWordId);
            return View(ruWord);
        }

        // GET: RuWords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RuWords == null)
            {
                return NotFound();
            }

            var ruWord = await _context.RuWords.FindAsync(id);
            if (ruWord == null)
            {
                return NotFound();
            }
            ViewData["EnWordId"] = new SelectList(_context.EnWords, "Id", "EnWriting", ruWord.EnWordId);
            return View(ruWord);
        }

        // POST: RuWords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RuWriting,EnWordId")] RuWord ruWord)
        {
            if (id != ruWord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruWord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuWordExists(ruWord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnWordId"] = new SelectList(_context.EnWords, "Id", "EnWriting", ruWord.EnWordId);
            return View(ruWord);
        }

        // GET: RuWords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RuWords == null)
            {
                return NotFound();
            }

            var ruWord = await _context.RuWords
                .Include(r => r.EnWord)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruWord == null)
            {
                return NotFound();
            }

            return View(ruWord);
        }

        // POST: RuWords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RuWords == null)
            {
                return Problem("Entity set 'ApplicationContext.RuWords'  is null.");
            }
            var ruWord = await _context.RuWords.FindAsync(id);
            if (ruWord != null)
            {
                _context.RuWords.Remove(ruWord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuWordExists(int id)
        {
          return (_context.RuWords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
