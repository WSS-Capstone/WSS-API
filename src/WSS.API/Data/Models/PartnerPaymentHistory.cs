﻿using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class PartnerPaymentHistory
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? OrderId { get; set; }
        public int? Status { get; set; }
        public double? Total { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Order? Order { get; set; }
        public virtual User? Partner { get; set; }
    }
}
