using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Task
    {
        public Task()
        {
            staff = new HashSet<staff>();
        }

        public Guid Id { get; set; }
        public Guid? StaffId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? OrderDetailId { get; set; }
        public string? TaskName { get; set; }
        public string? Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageEvidence { get; set; }
        public int? Status { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Owner? CreateByNavigation { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
