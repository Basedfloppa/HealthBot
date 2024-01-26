namespace HealthBot;

public partial class Generic
{
    public Guid Uuid { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTime UpdatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTime? DeletedAt { get; set; }
}
