using System.ComponentModel.DataAnnotations.Schema;

namespace ManyToMany.Core.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string SpielName { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public DateOnly ErscheingungsJahr { get; set; }
        public bool SinglePlayer { get; set; }
        public string Entwickler { get; set; }
        public ICollection<UserGame> UserGames { get; set; }

        [NotMapped]
        public List<string> SelectedPersonIds { get; set; } 
    }
}