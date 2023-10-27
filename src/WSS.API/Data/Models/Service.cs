using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Service
    {
        public Service()
        {
            ComboServices = new HashSet<ComboService>();
            CurrentPrices = new HashSet<CurrentPrice>();
            OrderDetails = new HashSet<OrderDetail>();
            ServiceImages = new HashSet<ServiceImage>();
        }

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public string? CoverUrl { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Unit { get; set; }
        public string? Reason { get; set; }
        public Guid? CreateBy { get; set; }
        public int? Quantity { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Account? CreateByNavigation { get; set; }
        public virtual ICollection<ComboService> ComboServices { get; set; }
        public virtual ICollection<CurrentPrice> CurrentPrices { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ServiceImage> ServiceImages { get; set; }
    }
}
