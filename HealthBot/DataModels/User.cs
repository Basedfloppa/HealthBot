using System;
using System.Collections.Generic;

namespace HealthBot;

public partial class User
{
    public Guid Uuid { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }

    public string? Alias { get; set; }

    public long ChatId { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public DateTime? SubscriptionEnd { get; set; }

    public DateTime? SubscriptionStart { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTimeOffset? UpdatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTimeOffset? DeletedAt { get; set; }

    public string LastAction { get; set; } = "";

    public virtual ICollection<Biometry> Biometries { get; set; } = new List<Biometry>();

    public virtual ICollection<Diaryentry> Diaryentries { get; set; } = new List<Diaryentry>();

    public virtual ICollection<Exportdatum> Exportdata { get; set; } = new List<Exportdatum>();

    public virtual ICollection<User> Observees { get; set; } = new List<User>();

    public virtual ICollection<User> Observers { get; set; } = new List<User>();
    public virtual ICollection<IntakeItem> IntakeItems { get; set; } = new List<IntakeItem>();

    public int messageid {get; set;}
}
