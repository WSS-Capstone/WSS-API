using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseCategoryNavigation = new HashSet<Category>();
            Partners = new HashSet<Partner>();
            Services = new HashSet<Service>();
            staff = new HashSet<staff>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Category? CategoryNavigation { get; set; }
        public virtual ICollection<Category> InverseCategoryNavigation { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
