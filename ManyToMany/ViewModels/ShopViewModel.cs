using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class ShopViewModel
    {
        public List<Game> AllGames { get; set; }      //game to schow
        public List<UserGame> MyGames { get; set; }   //for games in Korb
        public string SearchString { get; set; } = string.Empty;
    }
}
