using System;
using System.Collections.Generic;

namespace StarTrekWebAPI.Models;

public partial class Spacecraft
{
    public string? Uid { get; set; } = null;

    public string Name { get; set; } = null!;

    public string Registry { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string DateStatus { get; set; } = null!;

    public DateTime? SystemDate { get; set; } = null;

    public DateTime? LastChange { get; set; } = null;

    public bool? Deleted { get; set; } = null;
}
