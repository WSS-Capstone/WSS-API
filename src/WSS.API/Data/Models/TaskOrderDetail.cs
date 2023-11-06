using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class TaskOrderDetail
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid OrderDetailId { get; set; }

        public virtual OrderDetail OrderDetail { get; set; } = null!;
        public virtual Task Task { get; set; } = null!;
    }
}
