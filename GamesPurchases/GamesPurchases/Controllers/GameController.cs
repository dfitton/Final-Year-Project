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
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Game
        public IActionResult Index(int? s, string searchString) //int s is used to define the sorting method, string is used to search
        {
            ViewBag.Purchases = _context.Purchase.ToList(); // Gets all Purchases
            var Games = from g in _context.Game.ToList() select g; //Gets all Games

            if (s != null) // Doesn't search if a search isn't requested
            {
                switch (s) // Sorting method
                {
                    default:
                        Games = Games.OrderBy(g => g.GameID);
                        break;
                    case 1:     //Game Title A-Z
                        Games = Games.OrderBy(g => g.GameTitle);
                        break;
                    case 2:     //Game Title Z-A
                        Games = Games.OrderByDescending(g => g.GameTitle);
                        break;
                    case 3:     //Game Publisher A-Z
                        Games = Games.OrderBy(g => g.GamePublisher);
                        break;
                    case 4:     //Game Publisher Z-A
                        Games = Games.OrderByDescending(g => g.GamePublisher);
                        break;
                    case 5:     //Game Platform A-Z
                        Games = Games.OrderBy(g => g.GamePlatform);
                        break;
                    case 6:     //Game Platform Z-A
                        Games = Games.OrderByDescending(g => g.GamePlatform);
                        break;
                    case 7:     //Game Release Date A-Z
                        Games = Games.OrderBy(g => g.GameRelease);
                        break;
                    case 8:     //Game Release Date Z-A
                        Games = Games.OrderByDescending(g => g.GameRelease);
                        break;
                    case 9:     //Game RRP A-Z
                        Games = Games.OrderByDescending(g => g.GameRRP);
                        break;
                    case 10:     //Game RRP Z-A
                        Games = Games.OrderBy(g => g.GameRRP);
                        break;
                }
            }
            if(!String.IsNullOrEmpty(searchString)) // If a search is requested
            {
                Games = Games.Where(g => g.GameTitle.ToLower().Contains(searchString.ToLower()) //SQL Code that finds games with matching criteria
                                    || g.GamePlatform.ToString().ToLower().Contains(searchString.ToLower())
                                    || g.GamePublisher.ToLower().Contains(searchString.ToLower()));
            }
            ViewBag.Sort = s; //Used to modify the table headings
            return View(Games.ToList());
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Purchases = _context.Purchase.ToList();

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .SingleOrDefaultAsync(m => m.GameID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Game/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameID,GameTitle,GamePublisher,GamePlatform,GameRelease,GameRRP")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.SingleOrDefaultAsync(m => m.GameID == id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameID,GameTitle,GamePublisher,GamePlatform,GameRelease,GameRRP")] Game game)
        {
            if (id != game.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameID))
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
            return View(game);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Purchases = _context.Purchase.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .SingleOrDefaultAsync(m => m.GameID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.SingleOrDefaultAsync(m => m.GameID == id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameID == id);
        }
    }
}
