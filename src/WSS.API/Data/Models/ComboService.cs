using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class ComboService
    {
        public Guid Id { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ComboId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Combo? Combo { get; set; }
        public virtual Service? Service { get; set; }
    }
}
