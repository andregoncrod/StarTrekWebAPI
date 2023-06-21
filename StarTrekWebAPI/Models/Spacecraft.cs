using System;
using System.Collections.Generic;

namespace StarTrekWebAPI.Models;

public partial class Spacecraft
{
    public string Uid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Registry { get; set; }

    public string? Status { get; set; }

    public string? DateStatus { get; set; }

    public DateTime SystemDate { get; set; }

    public DateTime? LastChange { get; set; }

    public bool Deleted { get; set; }
}
