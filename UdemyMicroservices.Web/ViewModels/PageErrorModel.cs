namespace UdemyMicroservices.Web.ViewModels;

public record PageErrorModel
{
    public PageErrorModel(string? title, string? description)
    {
        SetTitle(title);
        SetDescription(description);
    }

    public PageErrorModel()
    {
    }

    public string? Title { get; set; }
    public string? Description { get; set; }

    public bool HasDescription => !string.IsNullOrEmpty(Description);


    public void SetTitle(string? title)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
    }

    public void SetDescription(string? description)
    {
        if (!string.IsNullOrEmpty(description)) Description = description;
    }
}