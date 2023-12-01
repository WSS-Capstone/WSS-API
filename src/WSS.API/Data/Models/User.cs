﻿using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            DayOffs = new HashSet<DayOff>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            PartnerPaymentHistories = new HashSet<PartnerPaymentHistory>();
            TaskCreateByNavigations = new HashSet<Task>();
            TaskPartners = new HashSet<Task>();
            TaskStaffs = new HashSet<Task>();
            Vouchers = new HashSet<Voucher>();
        }

        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Gender { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Account IdNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<DayOff> DayOffs { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }
        public virtual ICollection<Task> TaskCreateByNavigations { get; set; }
        public virtual ICollection<Task> TaskPartners { get; set; }
        public virtual ICollection<Task> TaskStaffs { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
