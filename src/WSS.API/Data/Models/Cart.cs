using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Cart
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ServiceId { get; set; }

        public virtual Service? Service { get; set; }
        public virtual Customer? User { get; set; }
    }
}
