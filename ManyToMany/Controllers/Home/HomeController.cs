using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ManyToMany.ViewModels;
using System.Threading.Tasks;

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
            var usersGames = new List<UserGame>();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                usersGames = await _context.UserGames
                                           .Where(ug => ug.PersonId == user.Id)
                                           .ToListAsync();
            }

            var model = new ShopViewModel
            {
                AllGames = await gamesQuery.ToListAsync(),
                UsersGames = usersGames,
                SearchString = searchString
            };

            return View(model);
        }
        //Giving back game
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
            var gameFromDB = await _context.Games
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.GameID == gameId);
            if (gameFromDB == null) return NotFound();
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
                PurchaseDate = DateTime.Now,
                SnapShotSpielName = gameFromDB.SpielName,
                SnapShotEntwickler = gameFromDB.Entwickler,
                SnapShotGenres = string.Join(", ", gameFromDB.Genres.Select(g => g.GenreName))
            };

            _context.UserGames.Add(purchase);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Das Game ist gekauft!";
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> WarenKorb()
        {
            var user = await _userManager.GetUserAsync(User);
            var users = await _context.Users.ToListAsync();
            var purchases = await _context.UserGames
       .Where(ug => ug.PersonId == user.Id)
       .ToListAsync();


            return View(purchases);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TransferGame(int gameID, string receiverEmail)
        {
            var sender = await _userManager.GetUserAsync(User);
            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            if (receiver == null)
            {
                TempData["Error"] = "Empfänger nicht gefunden.";
                return RedirectToAction("WarenKorb");
            }
            var sendersPurchase = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.PersonId == sender.Id && ug.GameId == gameID);
            if (sendersPurchase == null)
            {
                TempData["Error"] = "Sie besitzen dieses Spiel nicht.";
                return RedirectToAction("WarenKorb");
            }

            bool receiverAlreadyHasGame = await _context.UserGames
                .AnyAsync(ug => ug.PersonId == receiver.Id && ug.GameId == gameID);
            if (receiverAlreadyHasGame)
            {
                TempData["Message"] = "Der Empfänger besitzt dieses Spiel bereits.";
                return RedirectToAction("Index");
            }
            var newPuschase = new UserGame()
            {
                PersonId = receiver.Id,
                GameId = gameID,
                PurchaseDate = sendersPurchase.PurchaseDate,
                SnapShotSpielName = sendersPurchase.SnapShotSpielName,
                SnapShotEntwickler = sendersPurchase.SnapShotEntwickler,
                SnapShotGenres = sendersPurchase.SnapShotGenres
            };
            await _context.UserGames.AddAsync(newPuschase);
            _context.UserGames.Remove(sendersPurchase);
            await _context.SaveChangesAsync();
             TempData["Message"] = $"Spiel erfolgreich übertragen zu spiler {receiver.FirstName}";

            return RedirectToAction("Index");
        }
    }
}