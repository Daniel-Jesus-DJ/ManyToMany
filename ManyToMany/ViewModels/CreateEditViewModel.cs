using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class CreateEditViewModel
    {
        public List<Genre> Genres { get; set; }
        public Game Game { get; set; }
        public List<Game> Games { get; set; }
    }
}
