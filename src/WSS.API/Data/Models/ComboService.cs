using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class ComboService
    {
        public Guid Id { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ComboId { get; set; }

        public virtual Combo? Combo { get; set; }
        public virtual Service? Service { get; set; }
    }
}
