using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RecurringService
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public int LocationId { get; set; }

    public string Type { get; set; } = null!;

    public string Frequency { get; set; } = null!;
    public string CustomDays { get; set; }

    public TimeOnly? StartTime { get; set; }
}
