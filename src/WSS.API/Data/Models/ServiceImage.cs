using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class ServiceImage
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? ServiceId { get; set; }

        public virtual Service? Service { get; set; }
    }
}
