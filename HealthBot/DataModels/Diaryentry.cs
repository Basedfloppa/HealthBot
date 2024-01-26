using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Diaryentry: Generic
{
    public Guid Author { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public string Name { get; set; } = "";
    
    public string? Tags { get; set; }

    public string Type { get; set; } = null!;

    public int? CaloryAmount { get; set; }

    public string State { get; set; } = "";

    public int? Weight { get; set; }

    public int? HeartRate { get; set; }

    public int? BloodSaturation { get; set; }

    public string? BloodPreassure { get; set; }

}