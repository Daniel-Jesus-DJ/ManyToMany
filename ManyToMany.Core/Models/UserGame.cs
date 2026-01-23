using System;
using System.ComponentModel.DataAnnotations;

namespace ManyToMany.Core.Models
{
    public class UserGame
    {
        [Key]
        public int GameLicenceId { get; set; }
        public string PersonId { get; set; }
        public Person Person { get; set; }


        public int GameId { get; set; }
        public Game Game { get; set; }

        public string SpielName{ get; set; } = string.Empty;
        public string Entwickler { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;


        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}