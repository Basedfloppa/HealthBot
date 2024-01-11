using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class IntakeItem
{
    public Guid Uuid { get; set; }

    public string Name { get; set; } = null!;

    public int? CaloryAmount { get; set; }

    public string? Tags { get; set; }

    public string State { get; set; } = null!;

    public int? Weight { get; set; }

    public Guid DiaryEntry { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DeletedAt { get; set; }

    public virtual Diaryentry DiaryEntryNavigation { get; set; } = null!;
}
