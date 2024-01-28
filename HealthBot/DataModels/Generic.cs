namespace HealthBot;

public partial class Generic
{
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();

    public DateTime UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    
}
