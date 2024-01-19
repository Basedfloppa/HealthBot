using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Exportdatum
{
    public Guid Uuid { get; set; } = Guid.NewGuid();

    public Guid Author { get; set; }

    public string ExportedData { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;
}
