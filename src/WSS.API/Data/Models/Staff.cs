using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class staff
    {
        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? Gender { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Task Id1 { get; set; } = null!;
        public virtual Account IdNavigation { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
