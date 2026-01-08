using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Person> Users { get; set; }
        public List<Game> Games { get; set; }
        public List<Genre> Genres { get; set; }

        //N.M tables to demostrate
        public Dictionary<string, int> GamesPopularity { get; set; } // Игра -> Кол-во покупок
        public Dictionary<string, int> UsersActivity { get; set; }   // Юзер -> Кол-во купленных игр
    }
}
