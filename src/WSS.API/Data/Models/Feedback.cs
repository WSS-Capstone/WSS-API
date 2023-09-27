using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Rating { get; set; }
        public Guid? OrderDetailId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }

        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual Customer? User { get; set; }
    }
}
