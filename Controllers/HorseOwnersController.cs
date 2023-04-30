using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Controllers
{
    public class HorseOwnersController : Controller
    {
        private readonly ApplicationContext _context;

        public HorseOwnersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: HorseOwners
        public async Task<IActionResult> Index()
        {
              return _context.HorseOwners != null ? 
                          View(await _context.HorseOwners.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.HorseOwners'  is null.");
        }

        // GET: HorseOwners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HorseOwners == null)
            {
                return NotFound();
            }

            var horseOwnerModel = await _context.HorseOwners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horseOwnerModel == null)
            {
                return NotFound();
            }

            return View(horseOwnerModel);
        }

        // GET: HorseOwners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HorseOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone")] HorseOwnerModel horseOwnerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horseOwnerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horseOwnerModel);
        }

        // GET: HorseOwners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HorseOwners == null)
            {
                return NotFound();
            }

            var horseOwnerModel = await _context.HorseOwners.FindAsync(id);
            if (horseOwnerModel == null)
            {
                return NotFound();
            }
            return View(horseOwnerModel);
        }

        // POST: HorseOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Phone")] HorseOwnerModel horseOwnerModel)
        {
            if (id != horseOwnerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horseOwnerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseOwnerModelExists(horseOwnerModel.Id))
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
            return View(horseOwnerModel);
        }

        // GET: HorseOwners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HorseOwners == null)
            {
                return NotFound();
            }

            var horseOwnerModel = await _context.HorseOwners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horseOwnerModel == null)
            {
                return NotFound();
            }

            return View(horseOwnerModel);
        }

        // POST: HorseOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HorseOwners == null)
            {
                return Problem("Entity set 'ApplicationContext.HorseOwners'  is null.");
            }
            var horseOwnerModel = await _context.HorseOwners.FindAsync(id);
            if (horseOwnerModel != null)
            {
                _context.HorseOwners.Remove(horseOwnerModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseOwnerModelExists(int id)
        {
          return (_context.HorseOwners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
