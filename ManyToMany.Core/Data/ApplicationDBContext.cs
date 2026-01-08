using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ManyToMany.Core.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            optionsBuilder.UseSqlServer("Server=db37601.public.databaseasp.net; Database=db37601; User Id=db37601; Password=E+o3%a8GzA=7; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;", sqlOptions => sqlOptions.EnableRetryOnFailure());

            return new ApplicationDBContext(optionsBuilder.Options);
        }
    }

    // Наследуемся от IdentityDbContext<Person>
    public class ApplicationDBContext : IdentityDbContext<Person>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserGame> UserGames { get; set; } // Наша таблица покупок

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Не удаляй это! Нужно для Identity.

            // Настройка Many-to-Many для Игр и Пользователей через UserGame
            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.PersonId, ug.GameId }); // Составной ключ

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Person)
                .WithMany(p => p.UserGames)
                .HasForeignKey(ug => ug.PersonId);

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId);

            // Жанры оставляем как было (простая связь)
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Games)
                .WithMany(gm => gm.Genres);
        }
    }
}