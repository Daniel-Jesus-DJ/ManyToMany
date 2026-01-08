using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Person> Users { get; set; }
        public List<Game> Games { get; set; }
        public List<Genre> Genres { get; set; }
        public List<UserGame> AllPurchases { get; set; } // Журнал покупок
    }
}
