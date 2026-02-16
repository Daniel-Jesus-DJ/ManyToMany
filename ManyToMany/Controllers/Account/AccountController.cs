using ManyToMany.Core.Data;
using ManyToMany.Core.Models; // Проверь namespace!
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManyToMany.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDBContext _context;


        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // --- РЕГИСТРАЦИЯ ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string name, string firstName, DateOnly alter, Geschlecht geschlecht, int status)
        {
            // Создаем пользователя вручную
            var user = new Person
            {
                UserName = email, // Логин = Email
                Email = email,
                Name = name,
                FirstName = firstName,
                Alter = alter,
                Geschlecht = geschlecht,
                Status = 1,
                ZuletztOnline = DateTime.Now

            };

            // Identity сама захэширует пароль и сохранит в БД
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Сразу входим в систему после регистрации
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Если ошибки (например, пароль простой) - показываем их
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // --- ВХОД (LOGIN) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Пытаемся войти
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            var user = await _userManager.FindByNameAsync(email);

            if (result.Succeeded)
            {
                if (user.Email == email && user.Status == 1)
                {
                    ModelState.AddModelError(string.Empty, "Dieser Nutzer ist deaktiviert, für weitere Informationen, kontaktieren Sie einen Admin.");
                    return View();
                }
                user.ZuletztOnline = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
           
            ModelState.AddModelError(string.Empty, "Incorrect login or passwort");
            return View();
        }

        // --- ВЫХОД (LOGOUT) ---
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}