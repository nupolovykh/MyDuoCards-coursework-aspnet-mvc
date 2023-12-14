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
            var applicationContext = _context.Dictionaries.Include(d => d.EuWord).Include(d => d.User).Include(d => d.DictionaryStatement);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Dictionaries/Details/5
        public async Task<IActionResult> Details(int? userId, int? enWordId)
        {
            if (userId == null || enWordId == null || _context.Dictionaries == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries
                .Include(d => d.EuWord)
                .Include(d => d.User)
                .Include(d => d.DictionaryStatement)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.EnWordId == enWordId);

            if (dictionary == null)
            {
                return NotFound();
            }

            return View(dictionary);
        }


        // GET: Dictionaries/Create
        public IActionResult Create()
        {
            ViewData["DictionaryStatements"] = new SelectList(_context.DictionaryStatements, "Id", "Name");
            ViewData["EnWord"] = new SelectList(_context.EnWords, "Id", "EnWriting");
            ViewData["User"] = new SelectList(_context.Users, "Id", "Login");
            return View();
        }

        // POST: Dictionaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EnWordId,DictionaryStatementId")] Dictionary dictionary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dictionary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnWord"] = new SelectList(_context.EnWords, "Id", "EnWriting", dictionary.EnWordId);
            ViewData["User"] = new SelectList(_context.Users, "Id", "Login", dictionary.UserId);
            return View(dictionary);
        }

        // GET: Dictionaries/Edit/5
        public async Task<IActionResult> Edit(int? userId, int? enWordId)
        {
            if (userId == null || enWordId == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries.FirstOrDefaultAsync(d => d.UserId == userId && d.EnWordId == enWordId);

            if (dictionary == null)
            {
                return NotFound();
            }

            ViewData["DictionaryStatements"] = new SelectList(_context.DictionaryStatements, "Id", "Name");
            ViewData["EnWord"] = new SelectList(_context.EnWords, "Id", "EnWriting", dictionary.EnWordId);
            ViewData["User"] = new SelectList(_context.Users, "Id", "Login", dictionary.UserId);

            return View(dictionary);
        }

        // POST: Dictionaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, int enWordId, [Bind("UserId,EnWordId,DictionaryStatementId")] Dictionary dictionary)
        {
            if (userId != dictionary.UserId || enWordId != dictionary.EnWordId)
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
                    if (!DictionaryExists(dictionary.UserId, dictionary.EnWordId))
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
            ViewData["EnWord"] = new SelectList(_context.EnWords, "Id", "EnWriting", dictionary.EnWordId);
            ViewData["User"] = new SelectList(_context.Users, "Id", "Login", dictionary.UserId);
            return View(dictionary);
        }

        // GET: Dictionaries/Delete/5
        public async Task<IActionResult> Delete(int? userId, int? enWordId)
        {
            if (userId == null || enWordId == null || _context.Dictionaries == null)
            {
                return NotFound();
            }

            var dictionary = await _context.Dictionaries
                .Include(d => d.EuWord)
                .Include(d => d.User)
                .Include(d => d.DictionaryStatement)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.EnWordId == enWordId);

            if (dictionary == null)
            {
                return NotFound();
            }

            return View(dictionary);
        }

        // POST: Dictionaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int enWordId)
        {
            var dictionary = await _context.Dictionaries
                .FirstOrDefaultAsync(d => d.UserId == userId && d.EnWordId == enWordId);

            if (dictionary != null)
            {
                _context.Dictionaries.Remove(dictionary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        private bool DictionaryExists(int userId, int enWordId)
        {
            return _context.Dictionaries.Any(d => d.UserId == userId && d.EnWordId == enWordId);
        }
    }
}
