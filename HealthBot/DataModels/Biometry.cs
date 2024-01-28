namespace HealthBot;

public partial class Biometry : Generic
{
    public Guid Uuid { get; set; }
    
    public long Author { get; set; }

    public int? Weight { get; set; }

    public int? Height { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;
}
