using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkachkiWebApp.Areas.user.Models;

namespace SkachkiWebApp.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DOB,Rating")] JokeyModel jokey)
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
                UserModel user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == jokey.Id && u.RoleId == 2);
                _context.Users.Remove(user);
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
