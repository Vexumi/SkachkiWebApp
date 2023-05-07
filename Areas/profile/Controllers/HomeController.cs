using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                ViewBag.role = "admin";
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
                ViewBag.role = "jokey";
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
                ViewBag.role = "howner";
                if (howner != null)
                {
                    return View("ProfileHowner", howner);
                }
            }

            return Unauthorized();
        }
    }
}
