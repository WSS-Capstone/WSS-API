﻿using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Customer
{
    public Guid Id { get; set; }

    public string? Fullname { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ImageUrl { get; set; }

    public int? Gender { get; set; }

    public virtual Account IdNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
