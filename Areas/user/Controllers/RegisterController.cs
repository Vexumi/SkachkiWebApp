using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkachkiWebApp.Areas.user.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SkachkiWebApp.Areas.user.Controllers
{
    [Area("user")]
    public class RegisterController : Controller
    {
        private readonly ApplicationContext _context;

        public RegisterController(ApplicationContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //GET: /howner
        [AllowAnonymous]
        public IActionResult Howner()
        {
            return View();
        }

        //POST: /howner
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Howner([Bind("Name,Email,Password,Address,Phone")] RegisterHownerModel regHD)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");
            if (_context.Users.Where(p => p.Email == regHD.Email).Any()) return View("UserAlreadyRegistered");
            HorseOwnerModel userHowner = new HorseOwnerModel { Name = regHD.Name, Address = regHD.Address, Phone = regHD.Phone };
            _context.HorseOwners.Add(userHowner);
            await _context.SaveChangesAsync();

            var u = await _context.HorseOwners.FirstOrDefaultAsync(p => p.Name == regHD.Name && p.Address == regHD.Address);
            int userId = u.Id;
            UserModel user = new UserModel { Email = regHD.Email, Password = regHD.Password, CreationDate = DateTime.Now, RoleId = 3, UserId = userId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        //GET: /jokey
        [AllowAnonymous]
        public IActionResult Jokey()
        {
            return View();
        }

        //POST: /jokey
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Jokey([Bind("Name,Email,Password,DOB")] RegisterJokeyModel regJ)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");
            if (_context.Users.Where(p => p.Email == regJ.Email).Any()) return View("UserAlreadyRegistered");
            JokeyModel userJokey = new JokeyModel { Name = regJ.Name, DOB = regJ.DOB, Rating = 0 };
            _context.Jokeys.Add(userJokey);
            await _context.SaveChangesAsync();

            var u = await _context.Jokeys.FirstOrDefaultAsync(p => p.Name == regJ.Name && p.DOB == regJ.DOB);
            int userId = u.Id;
            UserModel user = new UserModel { Email = regJ.Email, Password = regJ.Password, CreationDate = DateTime.Now, RoleId = 2, UserId = userId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
