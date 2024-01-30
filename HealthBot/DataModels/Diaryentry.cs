using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Diaryentry : Generic
{
    public Guid Uuid { get; set; }

    public long Author { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public string? Name { get; set; }

    public string? Tags { get; set; }

    public string Type { get; set; } = null!;

    public float? CaloryAmount { get; set; }

    public string? State { get; set; }

    public float? Weight { get; set; }

    public float? HeartRate { get; set; }

    public float? BloodSaturation { get; set; }

    public string? BloodPreassure { get; set; }
}
