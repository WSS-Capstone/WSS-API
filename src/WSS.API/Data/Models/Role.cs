using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            Partners = new HashSet<Partner>();
            staff = new HashSet<staff>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? IsUser { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
