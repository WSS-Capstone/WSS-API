using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Combo
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? DiscountValueCombo { get; set; }

    public double? TotalAmount { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<ComboService> ComboServices { get; set; } = new List<ComboService>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
