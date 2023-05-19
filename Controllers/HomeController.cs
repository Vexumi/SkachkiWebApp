using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkachkiWebApp.Models;
using SQLitePCL;
using System;
using System.Diagnostics;
using System.Net;
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

        public async Task<IActionResult> Index()
        {
            var competitons = await _context.Competitions.Include(c => c.Ippodrom).ToListAsync();
            competitons.Reverse();
            return View(competitons);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("getImage/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var competition = _context.Competitions.FirstOrDefault(g => g.Id == id); // Получаем изображение по его ID

            if (competition == null) // Проверяем наличие изображения в базе данных
            {
                return NotFound(); // Возвращаем ошибку
            }

            return File(competition.ImageData, "image/jpeg"); // Возвращаем изображение в браузер в формате файла с типом MIME "image/jpeg"
        }
    }
}