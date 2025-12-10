using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Core.Models
{
   public class Game
    {
        public int GameID { get; set; }
        public string SpielName { get; set; }
        public string Genre { get; set; }
        public DateOnly ErscheingungsJahr { get; set; }
        public bool SinglePlayer { get; set; }
        public string Entwickler { get; set; }
        public string Publisher { get; set; }
        public ICollection<Person>? Persons { get; set; }
        [NotMapped]
        public List<int> SelectedPersonIds { get; set; }
    }
}
