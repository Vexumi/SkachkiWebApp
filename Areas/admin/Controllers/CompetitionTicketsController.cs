using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SkachkiWebApp.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class CompetitionTicketsController : Controller
    {
        private readonly ApplicationContext _context;

        public CompetitionTicketsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: CompetitionTickets
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.CompetitionTickets.Include(c => c.Competition).Include(c => c.Horse).Include(c => c.Jokey);
            return View(await applicationContext.ToListAsync());
        }

        // GET: CompetitionTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CompetitionTickets == null)
            {
                return NotFound();
            }

            var competitionTicketModel = await _context.CompetitionTickets
                .Include(c => c.Competition)
                .Include(c => c.Horse)
                .Include(c => c.Jokey)
                .FirstOrDefaultAsync(m => m.TiketId == id);
            if (competitionTicketModel == null)
            {
                return NotFound();
            }

            return View(competitionTicketModel);
        }

        // GET: CompetitionTickets/Create
        public IActionResult Create()
        {
            ViewBag.Competitions = new SelectList(_context.Competitions, "Id", "Name");
            ViewBag.Horses = new SelectList(_context.Horses, "Id", "Id");
            ViewBag.Jokeys = new SelectList(_context.Jokeys, "Id", "Name");
            return View();
        }

        // POST: CompetitionTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TiketId,CompetitionId,HorseId,JokeyId,Result")] CompetitionTicketModel competitionTicketModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competitionTicketModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionTicketModel.CompetitionId);
            ViewData["HorseId"] = new SelectList(_context.Horses, "Id", "Id", competitionTicketModel.HorseId);
            ViewData["JokeyId"] = new SelectList(_context.Jokeys, "Id", "Id", competitionTicketModel.JokeyId);
            return View(competitionTicketModel);
        }

        // GET: CompetitionTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CompetitionTickets == null)
            {
                return NotFound();
            }

            var competitionTicketModel = await _context.CompetitionTickets.FindAsync(id);
            if (competitionTicketModel == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionTicketModel.CompetitionId);
            ViewData["HorseId"] = new SelectList(_context.Horses, "Id", "Id", competitionTicketModel.HorseId);
            ViewData["JokeyId"] = new SelectList(_context.Jokeys, "Id", "Id", competitionTicketModel.JokeyId);
            return View(competitionTicketModel);
        }

        // POST: CompetitionTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TiketId,CompetitionId,HorseId,JokeyId,Result")] CompetitionTicketModel competitionTicketModel)
        {
            if (id != competitionTicketModel.TiketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competitionTicketModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionTicketModelExists(competitionTicketModel.TiketId))
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
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionTicketModel.CompetitionId);
            ViewData["HorseId"] = new SelectList(_context.Horses, "Id", "Id", competitionTicketModel.HorseId);
            ViewData["JokeyId"] = new SelectList(_context.Jokeys, "Id", "Id", competitionTicketModel.JokeyId);
            return View(competitionTicketModel);
        }

        // GET: CompetitionTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CompetitionTickets == null)
            {
                return NotFound();
            }

            var competitionTicketModel = await _context.CompetitionTickets
                .Include(c => c.Competition)
                .Include(c => c.Horse)
                .Include(c => c.Jokey)
                .FirstOrDefaultAsync(m => m.TiketId == id);
            if (competitionTicketModel == null)
            {
                return NotFound();
            }

            return View(competitionTicketModel);
        }

        // POST: CompetitionTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CompetitionTickets == null)
            {
                return Problem("Entity set 'ApplicationContext.CompetitionTickets'  is null.");
            }
            var competitionTicketModel = await _context.CompetitionTickets.FindAsync(id);
            if (competitionTicketModel != null)
            {
                _context.CompetitionTickets.Remove(competitionTicketModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionTicketModelExists(int id)
        {
            return (_context.CompetitionTickets?.Any(e => e.TiketId == id)).GetValueOrDefault();
        }
    }
}
