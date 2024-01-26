using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Exportdatum: Generic
{
    public Guid Author { get; set; }

    public string ExportedData { get; set; } = null!;

    public virtual User AuthorNavigation { get; set; } = null!;
}
