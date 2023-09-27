using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Message
    {
        public Guid Id { get; set; }
        public Guid? ToId { get; set; }
        public Guid? FromId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
