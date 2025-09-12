namespace JuanApp.Application.Models;

public class SliderDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? RedirectUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
