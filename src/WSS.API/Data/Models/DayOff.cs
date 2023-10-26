using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class DayOff
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public Guid? PartnerId { get; set; }
        public string? Reason { get; set; }
        public DateTime? Day { get; set; }

        public virtual User? Partner { get; set; }
    }
}
