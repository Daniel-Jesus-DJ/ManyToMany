using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ManyToMany.ViewModels;

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
           
            var gamesQuery = _context.Games
                                     .Include(g => g.Genres)
                                     .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                gamesQuery = gamesQuery.Where(g => g.SpielName.Contains(searchString)
                                                || g.Genres.Any(gen => gen.GenreName.Contains(searchString)));
            }

          
            List<UserGame> myGames = new List<UserGame>();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    myGames = await _context.UserGames
                                            .Include(ug => ug.Game) // Подгружаем названия игр
                                            .Where(ug => ug.PersonId == user.Id)
                                            .OrderByDescending(ug => ug.PurchaseDate) // Свежие сверху
                                            .ToListAsync();
                }
            }

           
            var model = new ShopViewModel
            {
                AllGames = await gamesQuery.ToListAsync(),
                MyGames = myGames,
                SearchString = searchString
            };

            return View(model);
        }
        //(Отмена покупки)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReturnGame(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);

            
            var purchase = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.PersonId == user.Id && ug.GameId == gameId);

            if (purchase != null)
            {
                _context.UserGames.Remove(purchase);
                await _context.SaveChangesAsync();   
                TempData["Message"] = "Wurd gelöscht";
            }
            else
            {
                TempData["Error"] = "Ошибка: Игра не найдена в вашей библиотеке.";
            }

            return RedirectToAction("Index");
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

            TempData["Message"] = "Das Game ist gekauft!";
            return RedirectToAction("Index");
        }
    }
}