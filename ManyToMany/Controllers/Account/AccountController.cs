using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ManyToMany.Core.Models; // Проверь namespace!

namespace ManyToMany.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- РЕГИСТРАЦИЯ ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string name, string firstName, DateOnly alter, Geschlecht geschlecht)
        {
            // Создаем пользователя вручную
            var user = new Person
            {
                UserName = email, // Логин = Email
                Email = email,
                Name = name,
                FirstName = firstName,
                Alter = alter,
                Geschlecht = geschlecht
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

            if (result.Succeeded)
            {
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