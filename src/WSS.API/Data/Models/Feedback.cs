using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Rating { get; set; }
        public Guid? OrderDetailId { get; set; }
        public int? Status { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual User? CreateByNavigation { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
    }
}
