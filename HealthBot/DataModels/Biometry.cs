using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Biometry
{
    public Guid Uuid { get; set; }

    public Guid Author { get; set; }

    public int? Weight { get; set; }

    public int? Height { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ChangedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;
}
