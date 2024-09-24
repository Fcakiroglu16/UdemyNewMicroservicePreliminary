﻿namespace UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel
{
    public record CourseViewModel(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string PictureUrl,
        string CategoryName,
        string Created,
        int Rating,
        int Duration);
}