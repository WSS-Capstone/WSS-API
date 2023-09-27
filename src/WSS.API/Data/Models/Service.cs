using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Service
    {
        public Service()
        {
            Carts = new HashSet<Cart>();
            ComboServices = new HashSet<ComboService>();
            CurrentPrices = new HashSet<CurrentPrice>();
            OrderDetails = new HashSet<OrderDetail>();
            PartnerServices = new HashSet<PartnerService>();
            ServiceImages = new HashSet<ServiceImage>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public string? CoverUrl { get; set; }
        public Guid? Categoryid { get; set; }
        public Guid? OwnerId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ComboService> ComboServices { get; set; }
        public virtual ICollection<CurrentPrice> CurrentPrices { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PartnerService> PartnerServices { get; set; }
        public virtual ICollection<ServiceImage> ServiceImages { get; set; }
    }
}
