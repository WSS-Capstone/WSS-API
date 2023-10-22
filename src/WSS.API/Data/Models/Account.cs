using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Account
    {
        public Account()
        {
            Services = new HashSet<Service>();
        }

        public string? Code { get; set; }
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public int? Status { get; set; }
        public string? RefId { get; set; }
        public string? RoleName { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }
        public string? Reason { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
