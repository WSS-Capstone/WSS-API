﻿using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Owner
{
    public Guid Id { get; set; }

    public string? Fullname { get; set; }

    public DateTime? DataOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ImageUrl { get; set; }

    public int? Gender { get; set; }

    public virtual Account IdNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
