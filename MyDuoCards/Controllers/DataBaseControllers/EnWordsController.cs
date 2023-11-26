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

namespace MyDuoCards.Controllers.DataBaseControllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EnWordsController : Controller
    {
        private readonly ApplicationContext _context;

        public EnWordsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: EnWords
        public async Task<IActionResult> Index()
        {
              return _context.EnWords != null ? 
                          View(await _context.EnWords.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.EnWords'  is null.");
        }

        // GET: EnWords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: EnWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EnWriting")] EnWord enWord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enWord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enWord);
        }

        // GET: EnWords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnWriting")] EnWord enWord)
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
                return RedirectToAction(nameof(Index));
            }
            return View(enWord);
        }

        // GET: EnWords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool EnWordExists(int id)
        {
          return (_context.EnWords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
