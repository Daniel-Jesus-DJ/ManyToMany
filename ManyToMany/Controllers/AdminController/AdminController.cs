using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using ManyToMany.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDBContext context, UserManager<Person> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // admin
        public async Task<IActionResult> Index()
        {
            var userGames = await _context.UserGames
                .Include(ug => ug.Game)
                .Include(ug => ug.Person)
                .ToListAsync();

            var model = new AdminDashboardViewModel
            {
                Users = await _context.Users.ToListAsync(),
                Games = await _context.Games.Include(g => g.Genres).ToListAsync(),
                Genres = await _context.Genres.ToListAsync(),
                AllPurchases = userGames
                    .OrderByDescending(ug => ug.PurchaseDate)
                    .ToList()
            };

            var gameCounts = userGames
                .GroupBy(ug => ug.Game.SpielName)
                .ToDictionary(g => g.Key, g => g.Count());

            var userCounts = userGames
                .GroupBy(ug => ug.Person.Email)
                .ToDictionary(g => g.Key, g => g.Count());

            model.GamesPopularity = model.Games
                .ToDictionary(
                    g => g.SpielName,
                    g => gameCounts.TryGetValue(g.SpielName, out var count) ? count : 0
                )
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            model.UsersActivity = model.Users
                .ToDictionary(
                    u => u.Email,
                    u => userCounts.TryGetValue(u.Email, out var count) ? count : 0
                )
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            return View(model);
        }


        //Manage Gmes


        [HttpGet]
        public async Task<IActionResult> CreateGame()
        {
           var model = new CreateEditViewModel
            {
               Game = new Game(),
               Genres = await _context.Genres.ToListAsync()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateGame(Game game, int[] selectedGenreIds)
        {
            // Notmapped GenreID
            if (selectedGenreIds != null && selectedGenreIds.Length > 0)
            {
                game.Genres = new List<Genre>();
                foreach (var id in selectedGenreIds)
                {
                    var genre = await _context.Genres.FindAsync(id);
                    if (genre != null)
                    {
                        game.Genres.Add(genre);
                    }
                }
            }

          
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            var genres = await _context.Genres.ToListAsync();
            var model = new CreateEditViewModel
            {
                Genres = genres,
                Game = game
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game, int[] selectedGenreIds)
        {
            var currentGame = await _context.Games
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.GameID == game.GameID);
            if (currentGame != null)
                {
                currentGame.SpielName = game.SpielName;
                currentGame.ErscheingungsJahr = game.ErscheingungsJahr;
                currentGame.SinglePlayer = game.SinglePlayer;
                currentGame.Entwickler = game.Entwickler;
                // Update Genres
                currentGame.Genres.Clear();
                if (selectedGenreIds != null && selectedGenreIds.Length > 0)
                {
                    foreach (var id in selectedGenreIds)
                    {
                        var genre = await _context.Genres.FindAsync(id);
                        if (genre !=null)
                        {

                        currentGame.Genres.Add(genre);
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        //Manage Genre
        [HttpPost]
        public async Task<IActionResult> CreateGenre(string genreName)
        {
            if (!string.IsNullOrEmpty(genreName))
            {
                _context.Genres.Add(new Genre { GenreName = genreName });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // manage Users

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            return RedirectToAction("Index");
        }
    }
}