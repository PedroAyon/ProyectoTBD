using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Client.Domain.Model;

public partial class Employee
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre es demasiado largo")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, ErrorMessage = "Los apellidos son demasiado largos")]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = null!;

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("position")]
    [Required(ErrorMessage = "La posicion es requerida")]
    public string Position { get; set; } = null!;

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    public static List<string> positions = new List<string>() { "Administracion", "Intendencia" };
}
