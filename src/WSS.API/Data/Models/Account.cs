using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Status { get; set; }

    public string? RefId { get; set; }

    public Guid? RoleName { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual Partner? Partner { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
