using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public Guid? TaskId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Task? Task { get; set; }
    }
}
