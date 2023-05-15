using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public int Stock { get; set; }
}
