﻿using Refit;
using System.Net;
using UdemyMicroservices.Web.Extensions;
using UdemyMicroservices.Web.Pages.Instructor.Course.Dto;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.ViewModels;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace UdemyMicroservices.Web.Services
{
    public class CatalogService(
        ICatalogService catalogService,
        IFileService fileService,
        ILogger<CatalogService> logger)
    {
        public async Task<ServiceResult<List<CourseViewModel>>> GetAllCourses()
        {
            var coursesAsResult = await catalogService.GetAllCourses();

            if (!coursesAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(coursesAsResult.Error.Content!);

                return ServiceResult<List<CourseViewModel>>.Fail(
                    "Failed to retrieve course data. Please try again later.");
            }

            var courses = coursesAsResult.Content!.Data!;
            var categoriesViewModel = courses.Select(c => new CourseViewModel(c.Id, c.Name, c.Description,
                c.Price, c.Picture,
                new CategoryViewModel(c.Category.Id, c.Category.Name), c.CreatedTime.ToShortDateString(),
                c.Feature.Rating,
                c.Feature.Duration)).ToList();

            return ServiceResult<List<CourseViewModel>>.Success(categoriesViewModel);
        }


        public async Task<ServiceResult> CreateCourseAsync(CreateCourseViewModel model)
        {
            string newCreatedFilepath = string.Empty;

            if (model.PictureFormFile is not null && model.PictureFormFile.Length > 0)
            {
                await using var stream = model.PictureFormFile.OpenReadStream();
                var streamPart = new StreamPart(stream, model.PictureFormFile.FileName,
                    model.PictureFormFile.ContentType);


                var fileAsResult = await fileService.UploadFileAsync(streamPart);

                if (fileAsResult.IsFail)
                {
                    //TODO: logging
                    logger.LogInformation("");
                }

                newCreatedFilepath = fileAsResult.Data!.FilePath;
            }


            var createCourseRequest = new CreateCourseRequest(model.Name, model.Description, model.Price,
                newCreatedFilepath, model.CategoryId!.Value);

            var response = await catalogService.CreateCourseAsync(createCourseRequest);


            if (!response.IsSuccessStatusCode)

            {
                logger.LogProblemDetails(response.Error.Content!);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ServiceResult.FailFromProblemDetails(response.Error.Content!);
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ServiceResult.Fail("course creation failed. Please try again later.");
                }
            }


            return response.Content!;
        }


        public async Task<ServiceResult<List<CategoryViewModel>>> GetCategoryList()
        {
            var categoriesAsResult = await catalogService.GetCategoriesAsync();
            if (!categoriesAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(categoriesAsResult.Error.Content!);
                return ServiceResult<List<CategoryViewModel>>.Fail(
                    "Failed to retrieve category data. Please try again later.");
            }


            var categoryModelList = categoriesAsResult.Content!.Data!.Select(x => new CategoryViewModel(x.Id, x.Name))
                .ToList();
            return ServiceResult<List<CategoryViewModel>>.Success(categoryModelList);
        }


        public async Task<ServiceResult> DeleteCourseAsync(Guid courseId)
        {
            var courseAsResult = await catalogService.GetCourse(courseId);


            if (!courseAsResult.IsSuccessStatusCode)
            {
                return ServiceResult.FailFromProblemDetails(courseAsResult.Error!.Content!);
            }


            var courseToDeleteAsResult = await catalogService.DeleteCourseAsync(courseId);

            if (!courseToDeleteAsResult.IsSuccessStatusCode)
            {
                return ServiceResult.FailFromProblemDetails(courseToDeleteAsResult.Error.Content!);
            }


            if (string.IsNullOrEmpty(courseAsResult.Content!.Data!.Picture))
            {
                return ServiceResult.Success();
            }

            var response =
                await fileService.DeleteFileAsync(new DeleteFileRequest(courseAsResult.Content!.Data!.Picture));


            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(response.Error.Message, "course picture could  not be deleted");
            }

            return ServiceResult.Success();
        }


        public async Task<ServiceResult<CourseViewModel>> GetCourse(Guid courseId)
        {
            var response = await catalogService.GetCourse(courseId);

            if (!response.IsSuccessStatusCode)
            {
                return ServiceResult<CourseViewModel>.FailFromProblemDetails(response.Error.Content!);
            }


            var course = response.Content!.Data!;
            var courseViewModel = new CourseViewModel(course.Id, course.Name, course.Description,
                course.Price, course.Picture,
                new CategoryViewModel(course.Category.Id, course.Category.Name),
                course.CreatedTime.ToShortDateString(),
                course.Feature.Rating,
                course.Feature.Duration);

            return ServiceResult<CourseViewModel>.Success(courseViewModel);
        }

        public async Task<ServiceResult> UpdateCourseAsync(UpdateCourseViewModel model)
        {
            string? coursePictureUrl = model.PictureUrl;

            if (model.PictureFormFile is not null && model.PictureFormFile.Length > 0)
            {
                await using var stream = model.PictureFormFile.OpenReadStream();
                var streamPart = new StreamPart(stream, model.PictureFormFile.FileName,
                    model.PictureFormFile.ContentType);


                var fileAsResult = await fileService.UploadFileAsync(streamPart);

                if (fileAsResult.IsFail)
                {
                    //TODO: logging
                    logger.LogInformation("");
                }

                coursePictureUrl = fileAsResult.Data!.FilePath;
            }


            var updateCourseRequest = new UpdateCourseRequest(model.Id, model.Name, model.Description, model.Price,
                coursePictureUrl, model.CategoryId);


            var response = await catalogService.UpdateCourseAsync(updateCourseRequest);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(response.Error.Message, "Course could not be updated.");

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ServiceResult.FailFromProblemDetails(response.Error.Content!);
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ServiceResult.Fail("A system error occurred. Please try again later.");
                }
            }

            return ServiceResult.Success();
        }


        public async Task<ServiceResult<CourseStatisticsViewModel>> GetCourseStatistic()
        {
            var getAllCoursesAsResult = await catalogService.GetAllCourses();


            if (!getAllCoursesAsResult.IsSuccessStatusCode)
            {
                return ServiceResult<CourseStatisticsViewModel>.FailFromProblemDetails(getAllCoursesAsResult.Error!
                    .Content!);
            }

            CourseStatisticsViewModel courseStatistics = new();

            courseStatistics.CourseCount = getAllCoursesAsResult.Content!.Data!.Count;


            if (courseStatistics.CourseCount > 0)
            {
                var averageRating = getAllCoursesAsResult.Content!.Data!.Average(c => c.Feature.Rating);

                courseStatistics.AverageRating = Math.Round(averageRating, 1);
            }
            else
            {
                courseStatistics.AverageRating = 0;
            }


            return ServiceResult<CourseStatisticsViewModel>.Success(courseStatistics);
        }
    }
}