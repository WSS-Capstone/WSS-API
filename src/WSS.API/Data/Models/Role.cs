using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? IsUser { get; set; }

    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
