using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class ShopViewModel
    {
        public List<Game> AllGames { get; set; }      //game to schow
        public List<UserGame> UsersGames { get; set; }    //users with games to WarenKorb
        public string SearchString { get; set; } = string.Empty;
    }
}
