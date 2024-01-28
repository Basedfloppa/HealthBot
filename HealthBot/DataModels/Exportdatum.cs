using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Exportdatum : Generic
{
    public Guid Uuid { get; set; }
    public long Author { get; set; }

    public string ExportedData { get; set; } = null!;

    public virtual User AuthorNavigation { get; set; } = null!;
}
