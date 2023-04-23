using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Controllers
{
    public class JokeysController : Controller
    {
        private readonly ApplicationContext _context;

        public JokeysController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Jokeys
        public async Task<IActionResult> Index()
        {
              return _context.Jokeys != null ? 
                          View(await _context.Jokeys.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Jokeys'  is null.");
        }

        // GET: Jokeys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jokeys == null)
            {
                return NotFound();
            }

            var jokey = await _context.Jokeys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokey == null)
            {
                return NotFound();
            }

            return View(jokey);
        }

        // GET: Jokeys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokeys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,CreationDate,Name,DOB,Rating")] Jokey jokey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jokey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jokey);
        }

        // GET: Jokeys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jokeys == null)
            {
                return NotFound();
            }

            var jokey = await _context.Jokeys.FindAsync(id);
            if (jokey == null)
            {
                return NotFound();
            }
            return View(jokey);
        }

        // POST: Jokeys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,CreationDate,Name,DOB,Rating")] Jokey jokey)
        {
            if (id != jokey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jokey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeyExists(jokey.Id))
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
            return View(jokey);
        }

        // GET: Jokeys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jokeys == null)
            {
                return NotFound();
            }

            var jokey = await _context.Jokeys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokey == null)
            {
                return NotFound();
            }

            return View(jokey);
        }

        // POST: Jokeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jokeys == null)
            {
                return Problem("Entity set 'ApplicationContext.Jokeys'  is null.");
            }
            var jokey = await _context.Jokeys.FindAsync(id);
            if (jokey != null)
            {
                _context.Jokeys.Remove(jokey);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeyExists(int id)
        {
          return (_context.Jokeys?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
