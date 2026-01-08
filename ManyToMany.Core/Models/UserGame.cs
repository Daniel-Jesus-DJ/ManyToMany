using System;

namespace ManyToMany.Core.Models
{
    public class UserGame
    {
        // Внешний ключ на пользователя (Identity использует string!)
        public string PersonId { get; set; }
        public Person Person { get; set; }

        // Внешний ключ на игру
        public int GameId { get; set; }
        public Game Game { get; set; }

        // То самое поле, ради которого мы это делаем
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}