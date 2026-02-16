using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyToMany.Core.Models
{
   
    public class Person : IdentityUser
    {
     

        public string Name { get; set; } 
        public string FirstName { get; set; }
        public Geschlecht Geschlecht { get; set; }
        public DateOnly Alter { get; set; }
        public int Status { get; set; } = 0;
        public DateTime ZuletztOnline { get; set; }

        public ICollection<UserGame> UserGames { get; set; }

        [NotMapped]
        public List<int> SelectedGameIds { get; set; }
    }
}