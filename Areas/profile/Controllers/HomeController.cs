using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkachkiWebApp.Areas.profile.Models;
using SkachkiWebApp.Areas.user.Models;
using SQLitePCL;
using System.Security.Claims;

namespace SkachkiWebApp.Areas.profile.Controllers
{
    [Area("profile")]
    [Authorize(Roles ="admin,jokey,howner")]
    public class HomeController : Controller
    {
        ApplicationContext _context;
        private string userRole { get; set; }
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identities = HttpContext.User.Claims;
            var emailClaim = identities.FirstOrDefault(p => p.Type == ClaimTypes.Email);
            string email = emailClaim.Value;

            // if user signed in as admin
            if (HttpContext.User.IsInRole("admin"))
            {
                UserModel user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
                userRole = "admin";
                ViewBag.role = userRole;
                if (user != null)
                {
                    return View("ProfileAdmin", user);
                }
            }
            
            // if user signed in as jokey
            if (HttpContext.User.IsInRole("jokey"))
            {
                UserModel user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
                JokeyModel jokey = await _context.Jokeys.FirstOrDefaultAsync(p => p.Id == user.UserId);
                userRole = "jokey";
                ViewBag.role = userRole;
                if (jokey != null)
                {
                    return View("ProfileJokey", jokey);
                }
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
                    var horses = _context.Horses.Where(p => p.HorseOwnerId == howner.Id).AsEnumerable();
                    HownerViewModel viewModel = new HownerViewModel() { Id = howner.Id, Email = user.Email, Name = howner.Name, Address = howner.Address, Phone = howner.Phone, Horses = horses };
                    return View("ProfileHowner", viewModel);
                }
            }

            return Unauthorized();
        }

        // GET: /newhorse
        [Route("/profile/newhorse")]
        public async Task<IActionResult> AddHorse()
        {
            ViewBag.Sex = new SelectList(new string[] { "Жеребец", "Кобыла" });
            return View();
        }

        // POST: /newhorse
        [Route("/profile/newhorse")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="howner")]
        public async Task<IActionResult> AddHorse(HorseModel horseData)
        {
            var identities = HttpContext.User.Claims;
            var emailClaim = identities.FirstOrDefault(p => p.Type == ClaimTypes.Email);
            string email = emailClaim.Value;

            UserModel user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
            horseData.HorseOwnerId = user.UserId;
            _context.Horses.Add(horseData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: profile/EditHorse/5
        public async Task<IActionResult> EditHorse(int? id)
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
            ViewBag.HorseOwner = horseModel.HorseOwnerId;
            ViewBag.Sex = new SelectList(new string[] { "Жеребец", "Кобыла" });
            return View(horseModel);
        }

        // POST: profile/EditHorse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "howner")]
        public async Task<IActionResult> EditHorse(int id, [Bind("Id,Nickname,Sex,DOB,HorseOwnerId")] HorseModel horseModel)
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
            return RedirectToAction(nameof(Index));
        }

        // GET: profile/DeleteHorse/5
        public async Task<IActionResult> DeleteHorse(int? id)
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

        // POST: profile/DeleteHorse/5
        [HttpPost, ActionName("DeleteHorse")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "howner")]
        public async Task<IActionResult> DeleteHorseConfirmed(int id)
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
