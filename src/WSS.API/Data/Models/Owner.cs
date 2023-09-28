using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Owner
    {
        public Owner()
        {
            Orders = new HashSet<Order>();
            Services = new HashSet<Service>();
            Tasks = new HashSet<Task>();
            Vouchers = new HashSet<Voucher>();
        }

        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? Gender { get; set; }

        public virtual Account IdNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
