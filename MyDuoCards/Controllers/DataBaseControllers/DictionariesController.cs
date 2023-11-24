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
    public class DictionariesController : Controller
    {
        private readonly ApplicationContext _context;

        public DictionariesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Dictionaries
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Dictionaries.Include(d => d.EuWord).Include(d => d.User);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Dictionaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dictionaries == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries
                .Include(d => d.EuWord)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (dictionary == null)
            {
                return NotFound();
            }

            return View(dictionary);
        }

        // GET: Dictionaries/Create
        public IActionResult Create()
        {
            ViewData["EnWordId"] = new SelectList(_context.EuWords, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Dictionaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EnWordId")] Dictionary dictionary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dictionary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnWordId"] = new SelectList(_context.EuWords, "Id", "Id", dictionary.EnWordId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dictionary.UserId);
            return View(dictionary);
        }

        // GET: Dictionaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dictionaries == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return NotFound();
            }
            ViewData["EnWordId"] = new SelectList(_context.EuWords, "Id", "Id", dictionary.EnWordId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dictionary.UserId);
            return View(dictionary);
        }

        // POST: Dictionaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,EnWordId")] Dictionary dictionary)
        {
            if (id != dictionary.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dictionary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DictionaryExists(dictionary.UserId))
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
            ViewData["EnWordId"] = new SelectList(_context.EuWords, "Id", "Id", dictionary.EnWordId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dictionary.UserId);
            return View(dictionary);
        }

        // GET: Dictionaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dictionaries == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries
                .Include(d => d.EuWord)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (dictionary == null)
            {
                return NotFound();
            }

            return View(dictionary);
        }

        // POST: Dictionaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dictionaries == null)
            {
                return Problem("Entity set 'ApplicationContext.Dictionaries'  is null.");
            }
            var dictionary = await _context.Dictionaries.FindAsync(id);
            if (dictionary != null)
            {
                _context.Dictionaries.Remove(dictionary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DictionaryExists(int id)
        {
          return (_context.Dictionaries?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
