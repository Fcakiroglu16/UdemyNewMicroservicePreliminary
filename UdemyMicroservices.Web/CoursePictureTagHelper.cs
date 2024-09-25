﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Services;

namespace UdemyMicroservices.Web
{
    public class CoursePictureTagHelper(FileServiceOption fileServiceOption) : TagHelper
    {
        public string? Src { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            var imagePath = "/pictures/default-course-picture.jpeg";

            if (!string.IsNullOrEmpty(Src))
            {
                imagePath = $"{fileServiceOption.Address}/{Src}";
            }


            output.Attributes.SetAttribute("src", imagePath);
            return base.ProcessAsync(context, output);
        }
    }
}