using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkachkiWebApp.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class CompetitionsController : Controller
    {
        private readonly ApplicationContext _context;

        public CompetitionsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Competitions.Include(c => c.Ippodrom);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Ippodrom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            ViewBag.Ippodroms = new SelectList(_context.Ippodroms, "Id", "Address");
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,IppodromId,PublicationDate,Description,ImageData,Recruiting")] CompetitionModel competition, IFormFile file = null)
        {
            competition.PublicationDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                //проверяем, что файл существует и имеет размер больше нуля
                if (file != null && file.Length > 0)
                {
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)file.Length);
                    }
                    competition.ImageName = file.FileName;
                }
                competition.ImageData = imageData;

                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IppodromId"] = new SelectList(_context.Ippodroms, "Id", "Address", competition.IppodromId);
            return View(competition);
        }

        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            ViewBag.Ippodroms = new SelectList(_context.Ippodroms, "Id", "Address", competition.IppodromId);
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,IppodromId,PublicationDate,Description,ImageData,Recruiting")] CompetitionModel competition, IFormFile? file = null)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                //проверяем, что файл существует и имеет размер больше нуля
                if (file != null && file.Length > 0)
                {
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)file.Length);
                    }
                    competition.ImageName = file.FileName;
                    competition.ImageData = imageData;
                }

                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
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
            foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewData["IppodromId"] = new SelectList(_context.Ippodroms, "Id", "Address", competition.IppodromId);
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Ippodrom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Competitions == null)
            {
                return Problem("Entity set 'ApplicationContext.Competitions'  is null.");
            }
            var competition = await _context.Competitions.FindAsync(id);
            if (competition != null)
            {
                _context.Competitions.Remove(competition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionExists(int id)
        {
            return (_context.Competitions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
