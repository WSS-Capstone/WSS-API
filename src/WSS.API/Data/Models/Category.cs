using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            Services = new HashSet<Service>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public Guid? CommisionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }
        public bool? IsOrderLimit { get; set; }

        public virtual Commission? Commision { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
