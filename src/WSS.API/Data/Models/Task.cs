using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Task
    {
        public Task()
        {
            Comments = new HashSet<Comment>();
            TaskOrderDetails = new HashSet<TaskOrderDetail>();
        }

        public string? Code { get; set; }
        public Guid Id { get; set; }
        public Guid? StaffId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? OrderDetailId { get; set; }
        public string? TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageEvidence { get; set; }
        public int? Status { get; set; }
        public Guid? CreateBy { get; set; }
        public bool? IsDelay { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual User? CreateByNavigation { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual User? Partner { get; set; }
        public virtual User? Staff { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<TaskOrderDetail> TaskOrderDetails { get; set; }
    }
}
