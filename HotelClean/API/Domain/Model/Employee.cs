using System;
using System.Collections.Generic;

namespace API.Domain.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Status { get; set; }

    public string Position { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public static List<string> positions = new List<string>() { "Administracion", "Intendencia" };
}
