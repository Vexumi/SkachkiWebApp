using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SkachkiWebApp.Areas.profile.Models;
using SkachkiWebApp.Areas.user.Models;
using System.Security.Claims;

namespace SkachkiWebApp.Areas.profile.Controllers
{
    [Area("profile")]
    [Authorize(Roles = "howner")]
    public class CompetitionRegistrationController : Controller
    {
        ApplicationContext _context;
        private string userRole { get; set; }
        public CompetitionRegistrationController(ApplicationContext context)
        {
            _context = context;
        }

        //GET
        public async Task<IActionResult> Index(int id)
        {
            var identities = HttpContext.User.Claims;
            var emailClaim = identities.FirstOrDefault(p => p.Type == ClaimTypes.Email);
            string email = emailClaim.Value;

            // if user signed in as admin or jokey
            if (HttpContext.User.IsInRole("admin") || HttpContext.User.IsInRole("jokey"))
            {
                userRole = "admin";
                ViewBag.role = userRole;
                return View();
            }
            // if user signed in as horse owner
            if (HttpContext.User.IsInRole("howner"))
            {
                UserModel user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
                HorseOwnerModel howner = await _context.HorseOwners.FirstOrDefaultAsync(p => p.Id == user.UserId);
                userRole = "howner";
                ViewBag.role = userRole;
                if (howner != null)
                {
                    ViewBag.Horses = new SelectList(await _context.Horses.Where(p => p.HorseOwnerId == howner.Id).ToListAsync(), "Id", "Nickname");
                    ViewBag.Jokeys = new SelectList(await _context.Jokeys.ToListAsync(), "Id", "Name");
                    ViewBag.Howner = howner;
                    var ticket = new CompetitionTicketModel() { CompetitionId = id };
                    return View(ticket);
                }
            }

            return Unauthorized();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("TicketId,CompetitionId,HorseId,JokeyId,Result")] CompetitionTicketModel cTicketData)
        {
            Console.WriteLine($"{cTicketData.CompetitionId} {cTicketData.HorseId} {cTicketData.JokeyId}");
            if (ModelState.IsValid)
            {
                if (_context.CompetitionTickets.
                    Where(c =>
                    (c.CompetitionId == cTicketData.CompetitionId && c.HorseId == cTicketData.HorseId)
                    ||
                    (c.CompetitionId == cTicketData.CompetitionId && c.JokeyId == cTicketData.JokeyId)
                    ).Any())
                {
                    return RedirectToAction(nameof(AlreadyRegistered));
                }
                _context.CompetitionTickets.Add(cTicketData);
                await _context.SaveChangesAsync();
                return Redirect("/");
            }
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Console.WriteLine(allErrors);

            var horse = await _context.Horses.FirstOrDefaultAsync(h => h.Id == cTicketData.HorseId);
            var hownerId = horse.HorseOwnerId;
            ViewBag.Horses = new SelectList(await _context.Horses.Where(p => p.HorseOwnerId == hownerId).ToListAsync(), "Id", "Nickname", cTicketData.HorseId);
            ViewBag.Jokeys = new SelectList(await _context.Jokeys.ToListAsync(), "Id", "Name", cTicketData.JokeyId);
            return View(cTicketData);
        }

        //TODO
        public IActionResult AlreadyRegistered()
        {
            return View();
        }
    }
}
