using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Areas.admin.Controllers
{
    [Area("admin")]
    public class IppodromsController : Controller
    {
        private ApplicationContext _context;
        public IppodromsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Ippodroms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ippodroms.ToListAsync());
        }

        // GET: Ippodroms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = await _context.Ippodroms.FirstOrDefaultAsync(m => m.Id == id);
            if (ippodrom == null)
            {
                return NotFound();
            }

            return View(ippodrom);
        }

        // GET: Ippodroms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ippodroms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Description")] IppodromModel ippodrom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ippodrom);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ippodrom);
        }

        // GET: Ippodroms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = await _context.Ippodroms.FindAsync(id);
            if (ippodrom == null)
            {
                return NotFound();
            }
            return View(ippodrom);
        }

        // POST: Ippodroms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Description")] IppodromModel ippodrom)
        {
            if (id != ippodrom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ippodrom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IppodromExists(ippodrom.Id))
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
            return View(ippodrom);
        }

        // GET: Ippodroms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = await _context.Ippodroms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ippodrom == null)
            {
                return NotFound();
            }

            return View(ippodrom);
        }

        // POST: Ippodroms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ippodroms == null)
            {
                return Problem("Entity set 'ApplicationContext.Ippodroms'  is null.");
            }
            var ippodrom = await _context.Ippodroms.FindAsync(id);
            if (ippodrom != null)
            {
                _context.Ippodroms.Remove(ippodrom);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Test if Ippodrom DB line exists
        private bool IppodromExists(int id)
        {
            return (_context.Ippodroms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
