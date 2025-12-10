using System.Diagnostics;
using ManyToMany.Core.Data;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public HomeController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.Include(g => g.Persons).ToListAsync();
            var persons = await _context.Persons.Include(p => p.Games).ToListAsync();
            GamePerson gamePerson = new GamePerson
            {
                Games = games,
                Persons = persons
            };
            return View(gamePerson);
        }

        // --- GAME CRUD OPERATIONS ---

        // GET: /Home/GetAllGames
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {

            var games = await _context.Games.Include(g => g.Persons).ToListAsync();
            return View(games);
        }

        // GET: /Home/GetGameById/{id}
        [HttpGet]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _context.Games.Include(g => g.Persons).FirstOrDefaultAsync(g => g.GameID == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST und Get: /Home/CreateGames

        [HttpGet]
        public async Task<IActionResult> CreateGames()
        {
            ViewBag.Persons = await _context.Persons
                .Select(p => new SelectListItem
                {
                    Value = p.PersonId.ToString(),
                    Text = p.Name
                })
                .ToListAsync();

            var model = new Game();
            model.SelectedPersonIds = new List<int>();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGames(Game game)
        {

            if (!ModelState.IsValid)
            {
                return View(game);
            }
           var selectedPersons = await _context.Persons
                .Where(p => game.SelectedPersonIds.Contains(p.PersonId))
                .ToListAsync();
            game.Persons = selectedPersons;

            await _context.Games.AddAsync(game);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        // PUT: /Home/UpdateGame/{id}
        public async Task<IActionResult> UpdateGame(int id)
        {
            var game = _context.Games
        .Include(g => g.Persons)
        .FirstOrDefault(g => g.GameID == id);

            ViewBag.Persons = _context.Persons
                .Select(p => new SelectListItem
                {
                    Value = p.PersonId.ToString(),
                    Text = p.Name,
                    Selected = game.Persons.Any(gp => gp.PersonId == p.PersonId)
                })
                .ToList();

            game.SelectedPersonIds = game.Persons
                .Select(p => p.PersonId)
                .ToList();

            return View(game);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGame(Game model)
        {
            var game = _context.Games
         .Include(g => g.Persons)
         .FirstOrDefault(g => g.GameID == model.GameID);

         
            game.SpielName = model.SpielName;
            game.Genre = model.Genre;
            game.ErscheingungsJahr = model.ErscheingungsJahr;
            game.SinglePlayer = model.SinglePlayer;
            game.Entwickler = model.Entwickler;
            game.Publisher = model.Publisher;

            //  Many-to-Many
            game.Persons.Clear();
            var selectedPersons = _context.Persons
                .Where(p => model.SelectedPersonIds.Contains(p.PersonId))
                .ToList();

            game.Persons = selectedPersons;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: /Home/DeleteGame/{id} 
        [HttpDelete]
        public async Task<IActionResult> DeleteGame(int id)
        {

            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // --- PERSON CRUD OPERATIONS ---

        // GET: /Home/GetAllPersons
        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {

            var persons = await _context.Persons.Include(p => p.Games).ToListAsync();
            return Ok(persons);
        }

        // GET: /Home/GetPerson/{id}
        [HttpGet]
        public async Task<IActionResult> GetPerson(int id)
        {

            var person = await _context.Persons
                                      .Include(p => p.Games)
                                      .FirstOrDefaultAsync(p => p.PersonId == id);

            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        // POST: /Home/CreatePerson
        [HttpGet]
        public async Task<IActionResult> CreatePerson()
        {
            ViewBag.Games = await _context.Games
                .Select(g => new SelectListItem
                {
                    Value = g.GameID.ToString(),
                    Text = g.SpielName
                })
                .ToListAsync();

            var model = new Person();
            model.SelectedGameIds = new List<int>();

            return View(model);
        }

       
        
        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
          var selectedGames = await _context.Games
                .Where(g => person.SelectedGameIds.Contains(g.GameID))
                .ToListAsync();
            person.Games= selectedGames;

            await _context.Persons.AddAsync(person);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");

        }

        // PUT: /Home/UpdatePerson/{id}
        [HttpPut]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person updatedPerson)
        {
            if (id != updatedPerson.PersonId || !ModelState.IsValid)
            {
                return BadRequest();
            }


            _context.Entry(updatedPerson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Persons.Any(e => e.PersonId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /Home/DeletePerson/{id}
        [HttpPost]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}