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
//        [HttpGet]
//        public async Task<IActionResult> CreateOrEditGenres(int? Id)
//        {
//            if (Id == 0 || Id == null)
//            {
//                // Create new person
//                return View(new Genre());
//            }
//            else
//            {
//                var existingGenre = await _context.Genres
//                    .Include(g => g.Games)
//                    .FirstOrDefaultAsync(g => g.Id == Id.Value);
//                if (existingGenre == null)
//                    {
//                    return NotFound();
//                }
//                return View(existingGenre);
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> CreateOrEditGenres(Genre genre)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(genre);
//            }
//            if (genre.Id == 0)
//            {
//                // Create new genre
//                _context.Genres.Add(genre);
//            }
//            else
//            {
//                // Update existing genre
//                var existingGenre = await _context.Genres.FindAsync(genre.Id);
//                if (existingGenre == null)
//                {
//                    return NotFound();
//                }
//                existingGenre.GenreName = genre.GenreName;
//                _context.Genres.Update(existingGenre);
//            }
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }
//    }
//}
