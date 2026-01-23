using ManyToMany.Core.Models;

namespace ManyToMany.ViewModels
{
    public class TransferViewModel
    {
        public  List<UserGame> Games { get; set; }
        public List<Person> Users { get; set; }
    }
}
