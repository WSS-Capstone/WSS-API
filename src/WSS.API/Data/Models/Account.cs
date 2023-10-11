using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Account
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public int? Status { get; set; }
        public string? RefId { get; set; }
        public string? RoleName { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual staff? staff { get; set; }
    }
}
