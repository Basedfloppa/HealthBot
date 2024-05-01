using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class Media: Generic
{
    public Guid Uuid { get; set; }

    public long ChatId { get; set; }

    public int MessageId { get; set; }

}
