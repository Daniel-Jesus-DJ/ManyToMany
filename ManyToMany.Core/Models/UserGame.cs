using System;

namespace ManyToMany.Core.Models
{
    public class UserGame
    {
     
        public string PersonId { get; set; }
        public Person Person { get; set; }


        public int GameId { get; set; }
        public Game Game { get; set; }

        public string SnapShotSpielName{ get; set; } = string.Empty;
        public string SnapShotEntwickler { get; set; } = string.Empty;
        public string SnapShotGenres { get; set; } = string.Empty;


        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}