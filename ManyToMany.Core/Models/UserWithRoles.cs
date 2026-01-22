using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Core.Models
{
    public class UserWithRoles
    {
        public string UserId;
        public IList<string> RoleName;
    }
}
