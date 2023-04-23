using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Controllers
{
    public class HorsesController : Controller
    {
        private readonly ApplicationContext _context;

        public HorsesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Horses
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Horses.Include(h => h.HorseOwner);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Horses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Horses == null)
            {
                return NotFound();
            }

            var horseModel = await _context.Horses
                .Include(h => h.HorseOwner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horseModel == null)
            {
                return NotFound();
            }

            return View(horseModel);
        }

        // GET: Horses/Create
        public IActionResult Create()
        {
            ViewBag.HorseOwners = new SelectList(_context.HorseOwners, "Id", "Name");
            ViewBag.Sex = new SelectList(new string[] { "Male", "Female" });
            return View();
        }

        // POST: Horses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nickname,Sex,DOB,HorseOwnerId")] HorseModel horseModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HorseOwnerId"] = new SelectList(_context.HorseOwners, "Id", "Name", horseModel.HorseOwnerId);
            return View(horseModel);
        }

        // GET: Horses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Horses == null)
            {
                return NotFound();
            }

            var horseModel = await _context.Horses.FindAsync(id);
            if (horseModel == null)
            {
                return NotFound();
            }
            ViewData["HorseOwnerId"] = new SelectList(_context.HorseOwners, "Id", "Id", horseModel.HorseOwnerId);
            return View(horseModel);
        }

        // POST: Horses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nickname,Sex,DOB,HorseOwnerId")] HorseModel horseModel)
        {
            if (id != horseModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseModelExists(horseModel.Id))
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
            ViewData["HorseOwnerId"] = new SelectList(_context.HorseOwners, "Id", "Id", horseModel.HorseOwnerId);
            return View(horseModel);
        }

        // GET: Horses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Horses == null)
            {
                return NotFound();
            }

            var horseModel = await _context.Horses
                .Include(h => h.HorseOwner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horseModel == null)
            {
                return NotFound();
            }

            return View(horseModel);
        }

        // POST: Horses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Horses == null)
            {
                return Problem("Entity set 'ApplicationContext.Horses'  is null.");
            }
            var horseModel = await _context.Horses.FindAsync(id);
            if (horseModel != null)
            {
                _context.Horses.Remove(horseModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseModelExists(int id)
        {
          return (_context.Horses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
