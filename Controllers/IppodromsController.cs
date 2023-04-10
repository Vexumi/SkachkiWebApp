using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Controllers
{
    public class IppodromsController : Controller
    {
        private ApplicationContext _context;
        public IppodromsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Ippodroms
        public IActionResult Index()
        {
            return View(_context.Ippodroms.ToList());
        }

        // GET: Ippodroms/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = _context.Ippodroms.FirstOrDefault(m => m.Id == id);
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
        public IActionResult Create([Bind("Id,Address,Description")] Ippodrom ippodrom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ippodrom);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ippodrom);
        }

        // GET: Ippodroms/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = _context.Ippodroms.Find(id);
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
        public IActionResult Edit(int id, [Bind("Id,Address,Description")] Ippodrom ippodrom)
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
                    _context.SaveChanges();
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
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Ippodroms == null)
            {
                return NotFound();
            }

            var ippodrom = _context.Ippodroms
                .FirstOrDefault(m => m.Id == id);
            if (ippodrom == null)
            {
                return NotFound();
            }

            return View(ippodrom);
        }

        // POST: Ippodroms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Ippodroms == null)
            {
                return Problem("Entity set 'ApplicationContext.Ippodroms'  is null.");
            }
            var ippodrom = _context.Ippodroms.Find(id);
            if (ippodrom != null)
            {
                _context.Ippodroms.Remove(ippodrom);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Test if Ippodrom DB line exists
        private bool IppodromExists(int id)
        {
            return (_context.Ippodroms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
