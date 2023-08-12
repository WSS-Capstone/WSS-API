using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ServiceId { get; set; }

    public Guid? PartnerId { get; set; }

    public string? Address { get; set; }

    public DateTime? StartTime { get; set; }

    public string? QuantityService { get; set; }

    public double? Price { get; set; }

    public double? Total { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }
}
