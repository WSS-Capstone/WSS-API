using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public Guid? UserId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public int? IsRead { get; set; }

        public virtual User? User { get; set; }
    }
}
