using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Config
    {
        public string Key { get; set; } = null!;
        public string? Value { get; set; }
    }
}
