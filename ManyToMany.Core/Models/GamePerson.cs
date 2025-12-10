using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Core.Models
{
    public class GamePerson
    {
        public IEnumerable<Game> Games { get; set; }
        public IEnumerable<Person> Persons { get; set; }
    }
}
