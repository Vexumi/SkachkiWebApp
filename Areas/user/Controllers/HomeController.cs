﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SkachkiWebApp.Areas.user.Models;

namespace SkachkiWebApp.Areas.user.Controllers
{
    [Area("user")]
    public class HomeController : Controller
    {

        ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Redirect("/");
        }

        //GET: /login
        [Route("/user/login")] // TODO
        public IActionResult Login()
        {
            return View();
        }

        //POST: /login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/user/login")] // TODO
        public async Task<IActionResult> Login(LoginModel loginData, string returnUrl)
        {
            if (string.IsNullOrEmpty(loginData.Password) || string.IsNullOrEmpty(loginData.Email)) return RedirectToAction("UserNotFound");
            UserModel? user = await _context.Users.FirstOrDefaultAsync(p => p.Email == loginData.Email && p.Password == loginData.Password);
            if (user is null) return RedirectToAction("UserNotFound");
            var role = await _context.Roles.FirstOrDefaultAsync(p => p.Id == user.RoleId);
            ClaimsIdentity? identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Email, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));

            var claimsPrincipal = new ClaimsPrincipal(identity);

            AuthenticationProperties authProperties;
            if (loginData.rememberMe) { 
                authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddMonths(2),
                    IsPersistent = true,
                };
            }
            else
            {
                authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.AddHours(12),
                    IsPersistent = true,
                };
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipal), authProperties);
            /*
            // проверяем, принадлежит ли URL приложению
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }*/
            if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);
            return Redirect("/Home");
        }

        //GET: /logout
        [Route("/user/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home");
        }

        [Route("/user/notfound")]
        public IActionResult UserNotFound()
        {
            return View();
        }

        [Route("/user/accessdenied")]
        public IActionResult UserAccessDenied()
        {
            return View();
        }
    }
}
