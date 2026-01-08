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
//        public async Task<IActionResult> CreateOrEditPersons(int? Id)
//        {
//            var allGames = await _context.Games.Select(g => new SelectListItem
//                {
//                    Value = g.GameID.ToString(),
//                    Text = g.SpielName
//                })
//                .ToListAsync();
//            if (Id == 0 || Id == null)
//            {
//                Person person = new Person
//                {
//                    SelectedGameIds = allGames.Select(g => int.Parse(g.Value)).ToList()
//                };
//                // Create new person
//                return View(person);
//            }
//            else
//            {
//                // Edit existing person
//                var person = await _context.Persons
//                    .Include(p => p.Games)
//                    .FirstOrDefaultAsync(p => p.PersonId == Id.Value);
//                if (person == null)
//                {
//                    return NotFound();
//                }
//                // Populate SelectedGameIds for the view model
//                person.SelectedGameIds = person.Games?.Select(g => g.GameID).ToList() ?? new List<int>();
//                return View(person);
//            }
//        }
//    }
//}
