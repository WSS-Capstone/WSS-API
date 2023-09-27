using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            Commissions = new HashSet<Commission>();
            InverseCategoryNavigation = new HashSet<Category>();
            Services = new HashSet<Service>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
        public Guid? CategoryId { get; set; }

        public virtual Category? CategoryNavigation { get; set; }
        public virtual ICollection<Commission> Commissions { get; set; }
        public virtual ICollection<Category> InverseCategoryNavigation { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
