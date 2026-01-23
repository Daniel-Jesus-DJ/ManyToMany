using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ManyToMany.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Person> Users { get; set; }
        public List<Game> Games { get; set; }
        public List<Genre> Genres { get; set; }
        public List<UserGame> AllPurchases { get; set; }
        public List<UserWithRoles> UsersWithRoles { get; set; }
      
        public List<GiftHistory> GiftHistory { get; set; }

        //N.M tables to demostrate
        public Dictionary<string, int> GamesPopularity { get; set; } 
        public Dictionary<string, int> UsersActivity { get; set; }   
    }
}
