using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Diaryentry
{
    public Guid Uuid { get; set; }

    public Guid Author { get; set; }

    public string Type { get; set; } = null!;

    public int? HeartRate { get; set; }

    public int? BloodSaturation { get; set; }

    public string? BloodPreassure { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DeletedAt { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual ICollection<IntakeItem> IntakeItems { get; set; } = new List<IntakeItem>();
}
