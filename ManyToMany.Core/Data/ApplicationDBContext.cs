using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyToMany.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ManyToMany.Core.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            optionsBuilder.UseSqlServer("Server=NB-Oskonbae-aad;Database=ManyToMany;Trusted_Connection=True;Encrypt=False;", sqlOptions => sqlOptions.EnableRetryOnFailure());

            return new ApplicationDBContext(optionsBuilder.Options);
        }
    }

    public class ApplicationDBContext:DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Game> Games { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Games)
                .WithMany(g => g.Persons);
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Persons)
                .WithMany(p => p.Games);
        }
    }
}
