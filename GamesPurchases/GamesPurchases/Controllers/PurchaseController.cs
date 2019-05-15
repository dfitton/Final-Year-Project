using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamesPurchases.Data;
using GamesPurchases.Models;

namespace GamesPurchases.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetGameTitleFromID(int i) // Used for sorting/searching to get the title of a game from its ID
        {
            string s = "";

            foreach (Game g in _context.Game.ToList())
            {
                if (g.GameID == i)
                {
                    s = g.GameTitle;
                }
            }
            return s;
        }

        private DateTime GetGameReleaseFromID(int i)// Used for sorting/searching to get the release date of a game from its ID
        {
            DateTime d = DateTime.Today;

            foreach (Game g in _context.Game.ToList())
            {
                if (g.GameID == i)
                {
                    d = g.GameRelease;
                }
            }
            return d;
        }

        private double GetGameRRPFromID(int i)// Used for sorting/searching to get the rrp of a game from its ID
        {
            double r = 0;

            foreach (Game g in _context.Game.ToList())
            {
                if (g.GameID == i)
                {
                    r = g.GameRRP;
                }
            }
            return r;
        }

        // GET: Purchase
        public IActionResult Index(int? s, string searchString) //int s is used to define the sorting method, string is used to search
        {
            ViewBag.Games = _context.Game.ToList(); //Gets all Games
            var Purchases = from p in _context.Purchase.ToList() select p; // Gets all Purchases

            if (s != null) // Doesn't search if a search isn't requested
            {
                switch (s) // Sorting method
                {
                    default: // Default search method
                        Purchases = Purchases.OrderBy(p => p.PurchaseID);
                        break;
                    case 1:     //Purchase Owner A-Z
                        Purchases = Purchases.OrderBy(p => p.PurchaseUser);
                        break;
                    case 2:     //Purchase Owner Z-A
                        Purchases = Purchases.OrderByDescending(p => p.PurchaseUser);
                        break;
                    case 3:     //Game Title A-Z
                        Purchases = Purchases.OrderBy(p => GetGameTitleFromID(p.PurchaseGame));
                        break;
                    case 4:     //Game Title Z-A
                        Purchases = Purchases.OrderByDescending(p => GetGameTitleFromID(p.PurchaseGame));
                        break;
                    case 5:     //Purchase Date A-Z
                        Purchases = Purchases.OrderBy(p => p.PurchaseDate);
                        break;
                    case 6:     //Purchase Date Z-A
                        Purchases = Purchases.OrderByDescending(p => p.PurchaseDate);
                        break;
                    case 7:     //Purchase Cost A-Z
                        Purchases = Purchases.OrderByDescending(p => p.PurchaseCost);
                        break;
                    case 8:     //Purchase Cost Z-A
                        Purchases = Purchases.OrderBy(p => p.PurchaseCost);
                        break;
                    case 9:     //Release Date A-Z
                        Purchases = Purchases.OrderBy(p => GetGameReleaseFromID(p.PurchaseGame));
                        break;
                    case 10:    //Release Date Z-A
                        Purchases = Purchases.OrderByDescending(p => GetGameReleaseFromID(p.PurchaseGame));
                        break;
                    case 11:    //Game RRP A-Z
                        Purchases = Purchases.OrderByDescending(p => GetGameRRPFromID(p.PurchaseGame));
                        break;
                    case 12:    //Game RRP Z-A
                        Purchases = Purchases.OrderBy(p => GetGameRRPFromID(p.PurchaseGame));
                        break;
                }
            }

            if (!String.IsNullOrEmpty(searchString)) // If a search is requested
            {
                Purchases = Purchases.Where(p => p.PurchaseUser.ToLower().Contains(searchString.ToLower()) //SQL Code that finds purchases with matching criteria
                                    || p.PurchaseDate.ToString().ToLower().Contains(searchString.ToLower())
                                    || GetGameTitleFromID(p.PurchaseGame).ToLower().Contains(searchString.ToLower())
                                    || GetGameReleaseFromID(p.PurchaseGame).ToShortDateString().Contains(searchString.ToLower()));
            }

            ViewBag.Sort = s; //Used to modify the table headings

            return View(Purchases.ToList());
        }

        // GET: Purchase/Details/5
        public async Task<IActionResult> Details(int? id) //Nothing changed from the default
        {
            ViewBag.Games = _context.Game.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .SingleOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchase/Create
        public IActionResult Create() //Nothing changed from the default
        {
            ViewBag.Games = _context.Game.OrderBy(g => g.GameTitle).ToList();
            return View();
        }

        // POST: Purchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseID,PurchaseUser,PurchaseGame,PurchaseDate,PurchaseCost,Refunded")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchase);
        }

        // GET: Purchase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Games = _context.Game.OrderBy(g => g.GameTitle).ToList();
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.SingleOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseID,PurchaseUser,PurchaseGame,PurchaseDate,PurchaseCost,Refunded")] Purchase purchase)
        {
            if (id != purchase.PurchaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.PurchaseID))
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
            return View(purchase);
        }

        // GET: Purchase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Games = _context.Game.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .SingleOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchase.SingleOrDefaultAsync(m => m.PurchaseID == id);
            _context.Purchase.Remove(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.PurchaseID == id);
        }
    }
}
