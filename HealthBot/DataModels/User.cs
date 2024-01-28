using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class User : Generic
{
    public string? Name { get; set; }

    public string? Alias { get; set; }

    public long ChatId { get; set; }

    public int MessageId { get; set; }

    public string? LastAction { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public DateTime? SubscriptionEnd { get; set; }

    public DateTime? SubscriptionStart { get; set; }

    public virtual ICollection<Biometry> Biometries { get; set; } = new List<Biometry>();

    public virtual ICollection<Diaryentry> DiaryEntries { get; set; } = new List<Diaryentry>();

    public virtual ICollection<Exportdatum> ExportData { get; set; } = new List<Exportdatum>();

    public virtual ICollection<User> Observees { get; set; } = new List<User>();

    public virtual ICollection<User> Observers { get; set; } = new List<User>();

}
