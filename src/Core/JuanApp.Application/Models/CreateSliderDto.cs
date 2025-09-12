namespace JuanApp.Application.Models;

public class CreateSliderDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? RedirectUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
