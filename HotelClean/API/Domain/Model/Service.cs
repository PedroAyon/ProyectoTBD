﻿using System;
using System.Collections.Generic;

namespace API.Domain.Model;

public partial class Service
{
    public int Id { get; set; }

    public int LocationId { get; set; }

    public string Type { get; set; } = null!;

    public string? Status { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndingTime { get; set; }
    public Location? Location { get; set; }
}
