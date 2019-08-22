using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamesPurchases.Models;
using GamesPurchases.Data;

namespace GamesPurchases.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() // Global Stats Page
        {
            ViewBag.Games = _context.Game.ToList();
            ViewBag.Purchases = _context.Purchase.ToList();
            ViewBag.UserCount = _context.Users.Count() -1 ;
            return View();
        }

        public IActionResult Personal() // Personal Stats Page
        {
            ViewBag.Games = _context.Game.ToList();
            ViewBag.Purchases = _context.Purchase.ToList();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
