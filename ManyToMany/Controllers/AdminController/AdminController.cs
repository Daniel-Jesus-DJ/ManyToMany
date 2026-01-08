using ManyToMany.Core.Data;
using ManyToMany.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Controllers
{
    // САМОЕ ВАЖНОЕ: Сюда пустит только если у юзера есть роль "Admin"
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AdminController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                Users = await _context.Users.ToListAsync(), // IdentityUser хранятся в Users
                Games = await _context.Games.Include(g => g.Genres).ToListAsync(),
                Genres = await _context.Genres.ToListAsync(),

                // Журнал покупок: кто, что и когда купил
                AllPurchases = await _context.UserGames
                    .Include(ug => ug.Person)
                    .Include(ug => ug.Game)
                    .OrderByDescending(ug => ug.PurchaseDate)
                    .ToListAsync()
            };

            return View(model);
        }

        // Сюда можно добавить методы DeleteUser, EditGame и т.д.
        // Например:
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}