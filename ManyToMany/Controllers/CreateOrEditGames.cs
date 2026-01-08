//using ManyToMany.Core.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//namespace ManyToMany.Controllers
//{
//    public partial class HomeController : Controller
//    {
//        [HttpGet]
//        public async Task<IActionResult> CreateOrEditGames(int? Id)
//        {
//            if (Id == 0 || Id == null)
//            {
//                var genres = await _context.Genres.ToListAsync();
//                var persons = await _context.Persons.ToListAsync();
//                Game game = new Game
//                {
//                    Genres = genres,
//                    SelectedPersonIds = persons.Select(p => p.PersonId).ToList()
//                };
//                // Create new game
//                return View(game);
//            }
//            else
//            {
//                // Edit existing game
//                var game = await _context.Games
//                    .Include(g => g.Persons).Include(g => g.Genres)
//                    .FirstOrDefaultAsync(g => g.GameID == Id.Value);
//                if (game == null)
//                {
//                    return NotFound();
//                }
//                // Populate SelectedPersonIds for the view model
//                game.SelectedPersonIds = game.Persons?.Select(p => p.PersonId).ToList() ?? new List<int>();
//                return View(game);
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> CreateOrEditGames(Game game)
//        {
//            if (!ModelState.IsValid)
//            {
//                var persons = await _context.Persons
//                .Select(p => new SelectListItem
//                {
//                    Value = p.PersonId.ToString(),
//                    Text = p.Name
//                })
//                .ToListAsync();
//                game.SelectedPersonIds = persons
//                    .Where(p => game.SelectedPersonIds.Contains(int.Parse(p.Value)))
//                    .Select(p => int.Parse(p.Value))
//                    .ToList();
//                return View(game);
//            }
//            var selectedPersons = await _context.Persons
//                .Where(p => game.SelectedPersonIds.Contains(p.PersonId))
//                .ToListAsync();
//            if (game.GameID == 0)
//            {
//                // Create new game
//                game.Persons = selectedPersons;
//                _context.Games.Add(game);
//            }
//            else
//            {
//                // Update existing game
//                var existingGame = await _context.Games
//                    .Include(g => g.Persons)
//                    .FirstOrDefaultAsync(g => g.GameID == game.GameID);
//                if (existingGame == null)
//                {
//                    return NotFound();
//                }
//                existingGame.SpielName = game.SpielName;
//                existingGame.ErscheingungsJahr = game.ErscheingungsJahr;
//                existingGame.SinglePlayer = game.SinglePlayer;
//                existingGame.Entwickler = game.Entwickler;
//                existingGame.Publisher = game.Publisher;
//                // Update associated persons
//                existingGame.Persons = selectedPersons;
//            }
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }
//    }
//}
