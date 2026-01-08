using System.ComponentModel.DataAnnotations.Schema;

namespace ManyToMany.Core.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string SpielName { get; set; }

        // Связь с жанрами (оставляем как было, если жанры простые)
        public ICollection<Genre> Genres { get; set; }

        public DateOnly ErscheingungsJahr { get; set; }
        public bool SinglePlayer { get; set; }
        public string Entwickler { get; set; }
        public string Publisher { get; set; }

        // Меняем связь с людьми на связь с "Покупками"
        public ICollection<UserGame> UserGames { get; set; }

        [NotMapped]
        public List<string> SelectedPersonIds { get; set; } // Тут теперь string, так как Id пользователя - строка
    }
}