using ManyToMany.Core.Data;
using ManyToMany.Core.Data; // Проверь namespace DbSeeder-а
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ManyToMany.Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- 2. РЕГИСТРАЦИЯ DB CONTEXT (Этого не хватало!) ---
// Без этой строки приложение падает с ошибкой "Unable to resolve service"
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

// --- 3. НАСТРОЙКА IDENTITY (С поддержкой РОЛЕЙ) ---
// AddDefaultIdentity не поддерживает роли по умолчанию. 
// Используем AddIdentity<Person, IdentityRole>, чтобы работал Админ.
builder.Services.AddIdentity<Person, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false; // Упростили пароли для теста
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

// Добавляем MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- 4. СОЗДАНИЕ АДМИНА ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Убедись, что класс DbSeeder существует и namespace верный
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ошибка создания админа: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())

{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- 5. ВАЖНЫЙ ПОРЯДОК MIDDLEWARE ---
app.UseAuthentication(); // <-- Обязательно добавь это (проверка "кто ты?")
app.UseAuthorization();  // <-- Потом это (проверка "можно ли тебе сюда?")

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();