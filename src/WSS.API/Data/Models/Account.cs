using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Account
    {
        public Account()
        {
            staff = new HashSet<staff>();
        }

        public Guid Id { get; set; }
        public string? Username { get; set; }
        public int? Status { get; set; }
        public string? RefId { get; set; }
        public string? RoleName { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
