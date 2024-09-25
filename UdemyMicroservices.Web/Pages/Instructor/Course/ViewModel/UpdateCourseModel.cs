using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;

public class UpdateCourseViewModel
{
    public static UpdateCourseViewModel Empty => new();


    public Guid Id { get; set; }

    [Display(Name = "Course Category")] public SelectList CategoryDropdownList { get; set; } = default!;


    [Display(Name = "Course Picture")] public IFormFile? PictureFormFile { get; set; }


    [Display(Name = "Course Name")] public string Name { get; set; } = default!;


    [Display(Name = "Course Description")] public string Description { get; set; } = default!;


    [Display(Name = "Course Price")] public decimal Price { get; set; }

    public string? PictureUrl { get; set; }

    public Guid CategoryId { get; set; }
}