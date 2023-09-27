using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class PartnerService
    {
        public Guid Id { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? ServiceId { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public string? ImageUrl { get; set; }
        public double? Priority { get; set; }

        public virtual Partner? Partner { get; set; }
        public virtual Service? Service { get; set; }
    }
}
