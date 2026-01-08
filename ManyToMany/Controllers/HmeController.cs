//using System.Diagnostics;
//using ManyToMany.Core.Data;
//using ManyToMany.Core.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//namespace ManyToMany.Controllers
//{
//    public partial class HomeController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        public HomeController(ApplicationDBContext context)
//        {
//            _context = context;
//        }


//        public async Task<IActionResult> Index()
//        {
//            var games = await _context.Games.Include(g => g.Persons).Include(g=>g.Genres).ToListAsync();
//            var persons = await _context.Persons.Include(p => p.Games).ToListAsync();
//          var genres = await _context.Genres.Include(g => g.Games).ToListAsync();
//            GamePerson gamePerson = new GamePerson
//            {
//                Games = games,
//                Persons = persons,
//                Genres = genres

//            };
//            return View(gamePerson);
//        }

//        // --- GAME CRUD OPERATIONS ---

//        // GET: /Home/GetAllGames
//        [HttpGet]
//        public async Task<IActionResult> GetAllGames()
//        {

//            var games = await _context.Games.Include(g => g.Persons).Include(g=>g.Genres).ToListAsync();
//            return View(games);
//        }

//        // GET: /Home/GetGameById/{id}
//        [HttpGet]
//        public async Task<IActionResult> GetGameById(int id)
//        {
//            var game = await _context.Games.Include(g => g.Persons).FirstOrDefaultAsync(g => g.GameID == id);

//            if (game == null)
//            {
//                return NotFound();
//            }

//            return View(game);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateGames(Game game)
//        {
//           if(!ModelState.IsValid)
//            {
//                ViewBag.Persons = await _context.Persons
//                .Select(p => new SelectListItem
//                {
//                    Value = p.PersonId.ToString(),
//                    Text = p.Name
//                })
//                .ToListAsync();
//                return View(game);
//            }
//           var selectedPersons = await _context.Persons
//                .Where(p => game.SelectedPersonIds.Contains(p.PersonId))
//                .ToListAsync();
//            game.Persons = selectedPersons;

//            await _context.Games.AddAsync(game);

//            await _context.SaveChangesAsync();


//            return RedirectToAction("Index");
//        }



//        // POST: /Home/DeleteGame/{id} 
//        [HttpDelete]
//        public async Task<IActionResult> DeleteGame(int id)
//        {

//            var game = await _context.Games.FindAsync(id);

//            if (game == null)
//            {
//                return NotFound();
//            }

//            _context.Games.Remove(game);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // --- PERSON CRUD OPERATIONS ---

//        // GET: /Home/GetAllPersons
//        [HttpGet]
//        public async Task<IActionResult> GetAllPersons()
//        {

//            var persons = await _context.Persons.Include(p => p.Games).ToListAsync();
//            return Ok(persons);
//        }

//        // GET: /Home/GetPerson/{id}
//        [HttpGet]
//        public async Task<IActionResult> GetPerson(int id)
//        {

//            var person = await _context.Persons
//                                      .Include(p => p.Games)
//                                      .FirstOrDefaultAsync(p => p.PersonId == id);

//            if (person == null)
//            {
//                return NotFound();
//            }
//            return Ok(person);
//        }

  

//        // POST: /Home/DeletePerson/{id}
//        [HttpPost]
//        public async Task<IActionResult> DeletePerson(int id)
//        {
//            var person = await _context.Persons.FindAsync(id);

//            if (person == null)
//            {
//                return NotFound();
//            }

//            _context.Persons.Remove(person);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // POST: /Home/DeleteGenre/{id}
//        [HttpPost]
//        public async Task<IActionResult> DeleteGenre(int id)
//        {
//            var genre = await _context.Genres.FindAsync(id);

//            if (genre == null)
//            {
//                return NotFound();
//            }

//            _context.Genres.Remove(genre);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
