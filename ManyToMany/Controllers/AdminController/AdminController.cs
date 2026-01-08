using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using ManyToMany.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Controllers
{
    [Authorize(Roles = "Admin")] // Доступ только админам
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

        // --- ГЛАВНАЯ СТРАНИЦА АДМИНА ---
        public async Task<IActionResult> Index()
        {
            // Подгружаем данные для статистики M:N
            var userGames = await _context.UserGames.Include(ug => ug.Game).Include(ug => ug.Person).ToListAsync();

            var model = new AdminDashboardViewModel
            {
                Users = await _userManager.Users.ToListAsync(),
                Games = await _context.Games.Include(g => g.Genres).ToListAsync(),
                Genres = await _context.Genres.ToListAsync(),

                // Считаем статистику (M:N во всей красе)
                GamesPopularity = userGames.GroupBy(x => x.Game.SpielName)
                                           .ToDictionary(g => g.Key, g => g.Count()),

                UsersActivity = userGames.GroupBy(x => x.Person.Email)
                                         .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(model);
        }

        // --- УПРАВЛЕНИЕ ИГРАМИ ---

        [HttpGet]
        public IActionResult CreateGame() => View();

        [HttpPost]
        public async Task<IActionResult> CreateGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
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

        // --- УПРАВЛЕНИЕ ЖАНРАМИ ---

        [HttpPost] // Быстрое создание жанра прямо с главной (сделаем форму в Index)
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

        // --- УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ ---

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

        // Назначить Админом (или снять)
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