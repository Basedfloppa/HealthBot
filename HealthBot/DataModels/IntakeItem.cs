using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class IntakeItem
{
    public Guid Uuid { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public int? CaloryAmount { get; set; }

    public string? Tags { get; set; }

    public string State { get; set; } = null!;

    public int? Weight { get; set; }

    public Guid Author { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTime UpdatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTime DeletedAt { get; set; }

    public virtual Diaryentry DiaryEntryNavigation { get; set; } = null!;
}
