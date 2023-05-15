using System;
using System.Collections.Generic;

namespace API.Domain.Model;

public partial class Employee
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}
