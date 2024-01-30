using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Admin : Generic
{
    public Guid Uuid { get; set; }

    public long User { get; set; }

    public int Level { get; set; }

}
