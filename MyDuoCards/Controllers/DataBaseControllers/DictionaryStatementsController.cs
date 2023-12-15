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
	public class DictionaryStatementsController : Controller
    {
        private readonly ApplicationContext _context;

        public DictionaryStatementsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: DictionaryStatements
        public async Task<IActionResult> Index()
        {
              return _context.DictionaryStatements != null ? 
                          View(await _context.DictionaryStatements.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.DictionaryStatements'  is null.");
        }

        // GET: DictionaryStatements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DictionaryStatements == null)
            {
                return NotFound();
            }

            var dictionaryStatement = await _context.DictionaryStatements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dictionaryStatement == null)
            {
                return NotFound();
            }

            return View(dictionaryStatement);
        }

        // GET: DictionaryStatements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DictionaryStatements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DictionaryStatement dictionaryStatement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dictionaryStatement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dictionaryStatement);
        }

        // GET: DictionaryStatements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DictionaryStatements == null)
            {
                return NotFound();
            }

            var dictionaryStatement = await _context.DictionaryStatements.FindAsync(id);
            if (dictionaryStatement == null)
            {
                return NotFound();
            }
            return View(dictionaryStatement);
        }

        // POST: DictionaryStatements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DictionaryStatement dictionaryStatement)
        {
            if (id != dictionaryStatement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dictionaryStatement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DictionaryStatementExists(dictionaryStatement.Id))
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
            return View(dictionaryStatement);
        }

        // GET: DictionaryStatements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DictionaryStatements == null)
            {
                return NotFound();
            }

            var dictionaryStatement = await _context.DictionaryStatements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dictionaryStatement == null)
            {
                return NotFound();
            }

            return View(dictionaryStatement);
        }

        // POST: DictionaryStatements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DictionaryStatements == null)
            {
                return Problem("Entity set 'ApplicationContext.DictionaryStatements'  is null.");
            }
            var dictionaryStatement = await _context.DictionaryStatements.FindAsync(id);
            if (dictionaryStatement != null)
            {
                _context.DictionaryStatements.Remove(dictionaryStatement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DictionaryStatementExists(int id)
        {
          return (_context.DictionaryStatements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
