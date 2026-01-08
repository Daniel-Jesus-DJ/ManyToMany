using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyToMany.Core.Models
{
    // Наследуемся от IdentityUser!
    public class Person : IdentityUser
    {
        // PersonId удаляем — он теперь в IdentityUser и называется просто "Id" (тип string)
        // Email удаляем — он тоже есть в IdentityUser

        public string Name { get; set; } // Можно оставить как фамилию
        public string FirstName { get; set; }
        public Geschlecht Geschlecht { get; set; }
        public DateOnly Alter { get; set; }

        // ВАЖНО: Для магазина лучше использовать явную связь через UserGame,
        // чтобы знать ДАТУ покупки.
        public ICollection<UserGame> UserGames { get; set; }

        [NotMapped]
        public List<int> SelectedGameIds { get; set; }
    }
}