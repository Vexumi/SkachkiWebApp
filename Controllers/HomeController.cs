using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkachkiWebApp.Models;
using SQLitePCL;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;

namespace SkachkiWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        //GET: /login
        [ActionName("login")]
        public IActionResult Login()
        {
            return View();
        }

        //POST: /login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("login")]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginModel loginData)
        {
            if (string.IsNullOrEmpty(loginData.Password) || string.IsNullOrEmpty(loginData.Email)) return Unauthorized();
            UserModel? user = await _context.Users.FirstOrDefaultAsync(p => p.Email == loginData.Email && p.Password == loginData.Password);
            if (user is null) return Unauthorized();
            var role = await _context.Roles.FirstOrDefaultAsync(p => p.Id == user.RoleId);
            ClaimsIdentity? identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Email, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));

            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(1),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipal), authProperties);
            return RedirectToAction("Index");
        }

        //GET: /logout
        [ActionName("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}