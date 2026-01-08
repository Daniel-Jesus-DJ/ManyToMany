using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ManyToMany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<Person> _userManager;

        public HomeController(ApplicationDBContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString; 

            var gamesQuery = _context.Games
                                     .Include(g => g.Genres) 
                                     .AsQueryable(); // Готовим запрос

            // Если строка поиска не пустая - добавляем фильтр
            if (!string.IsNullOrEmpty(searchString))
            {
                gamesQuery = gamesQuery.Where(g => g.SpielName.Contains(searchString)
                                                || g.Genres.Any(gen => gen.GenreName.Contains(searchString)));
            }

            // Выполняем запрос и возвращаем результат
            return View(await gamesQuery.ToListAsync());
        }

      
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);

            bool alreadyBought = await _context.UserGames
                .AnyAsync(ug => ug.PersonId == user.Id && ug.GameId == gameId);

            if (alreadyBought)
            {
                TempData["Message"] = "Sie haben schon!";
                return RedirectToAction("Index");
            }

            var purchase = new UserGame
            {
                PersonId = user.Id,
                GameId = gameId,
                PurchaseDate = DateTime.Now
            };

            _context.UserGames.Add(purchase);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Spiel ist gekauft!";
            return RedirectToAction("Index");
        }
    }
}