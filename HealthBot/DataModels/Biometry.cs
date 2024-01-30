namespace HealthBot;

public partial class Biometry : Generic
{
    public Guid Uuid { get; set; }

    public long Author { get; set; }

    public float? Weight { get; set; }

    public float? Height { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;
}
