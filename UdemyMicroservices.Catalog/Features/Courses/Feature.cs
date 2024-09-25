namespace UdemyMicroservices.Catalog.Features.Courses;

public class Feature
{
    public int Duration { get; set; }

    public int Rating { get; set; }

    public string EducatorFullName { get; set; } = default!;
}