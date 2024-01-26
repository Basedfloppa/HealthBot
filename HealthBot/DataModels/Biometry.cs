namespace HealthBot;

public partial class Biometry : Generic
{
    public Guid Author { get; set; }

    public int? Weight { get; set; }

    public int? Height { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;
}
