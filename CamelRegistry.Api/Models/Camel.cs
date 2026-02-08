namespace CamelRegistry.Api.Models;

public class Camel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Color { get; set; } = string.Empty;
    public int HumpCount { get; set; }
    public DateTime LastFed { get; set; }
}
