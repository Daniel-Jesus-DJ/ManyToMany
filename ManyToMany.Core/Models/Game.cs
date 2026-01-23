using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Builder;

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
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public List<string> SelectedPersonIds { get; set; } 
    }
}