using System;
using System.Collections.Generic;

namespace API.Domain.Model;

public partial class Location
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string Type { get; set; } = null!;

    public int Floor { get; set; }
}
