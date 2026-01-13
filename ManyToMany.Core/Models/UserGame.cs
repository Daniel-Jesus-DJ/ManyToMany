using System;

namespace ManyToMany.Core.Models
{
    public class UserGame
    {
     
        public string PersonId { get; set; }
        public Person Person { get; set; }


        public int GameId { get; set; }
        public Game Game { get; set; }
        
 
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}