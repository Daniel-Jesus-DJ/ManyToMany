using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Core.Models
{
    public class GiftHistory
    {
        public int Id { get; set; }
        public string? GameName { get; set; }
        public string? SenderName { get; set; }
        public string? RevieverName { get; set; }
        public DateTime SendingDate { get; set; }
    }
}
