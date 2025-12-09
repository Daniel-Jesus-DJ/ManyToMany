using System.Diagnostics;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Mvc;
using ManyToMany.Core.Data;
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

        public IActionResult Index()
        {
            return View();
        }

        // --- GAME CRUD OPERATIONS ---

        // GET: /Home/GetAllGames
        [HttpGet]
        public async Task<IActionResult> GetAllGames() 
        {
           
            var games = await _context.Games.Include(g=>g.Persons).ToListAsync();
            return Ok(games);
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

            return Ok(game);
        }

        // POST: /Home/CreateGames
        [HttpPost]
        public async Task<IActionResult> CreateGames([FromBody] Game game)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Games.AddAsync(game);
            
            await _context.SaveChangesAsync();

  
            return CreatedAtAction(nameof(GetGameById), new { id = game.GameID }, game);
        }

        // PUT: /Home/UpdateGame/{id}
        [HttpPut]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] Game updatedGame)
        {
            if (id != updatedGame.GameID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            //best way to use optimized update
            _context.Entry(updatedGame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            
                if (!_context.Games.Any(e => e.GameID == id))
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
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
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