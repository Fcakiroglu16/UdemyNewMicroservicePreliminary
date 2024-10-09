using Microsoft.AspNetCore.Razor.TagHelpers;

namespace UdemyMicroservices.Web.TagHelpers;

public class CoursePictureTagHelper(IConfiguration configuration) : TagHelper
{
    public string? Src { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";

        var imagePath = "/pictures/default-course-picture.jpeg";

        if (!string.IsNullOrEmpty(Src))
            imagePath = $"{configuration.GetSection("FileServiceOption")["PublishAddress"]}/{Src}";


        output.Attributes.SetAttribute("src", imagePath);
        return base.ProcessAsync(context, output);
    }
}