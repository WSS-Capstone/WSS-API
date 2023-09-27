using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class WeddingInformation
    {
        public WeddingInformation()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string? NameGroom { get; set; }
        public string? NameBride { get; set; }
        public string? NameBrideFather { get; set; }
        public string? NameBrideMother { get; set; }
        public string? NameGroomFather { get; set; }
        public string? NameGroomMother { get; set; }
        public DateTime? WeddingDay { get; set; }
        public string? ImageUrl { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
