using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;

namespace ManyToMany.Core.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Geschlecht { get; set; }
        public DateOnly Alter { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}