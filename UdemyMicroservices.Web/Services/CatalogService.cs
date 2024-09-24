using Refit;
using System.Net;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Services
{
    public class CatalogService(
        ICatalogService catalogService,
        IFileService fileService,
        FileServiceOption fileServiceOption,
        ILogger<CatalogService> logger)
    {
        public async Task<ServiceResult<List<CourseViewModel>>> GetAllCourses()
        {
            var getAllCoursesAsResult = await catalogService.GetAllCourses();

            if (!getAllCoursesAsResult.IsSuccessStatusCode)
            {
                return ServiceResult<List<CourseViewModel>>.Fail(getAllCoursesAsResult.Error!.Content!);
            }

            var courses = getAllCoursesAsResult.Content!.Data!;
            var categoriesViewModel = courses.Select(c => new CourseViewModel(c.Id, c.Name, c.Description,
                c.Price, $"{fileServiceOption.Address}/{c.Picture}", c.Category.Name, c.CreatedTime.ToShortDateString(),
                c.Feature.Rating,
                c.Feature.Duration)).ToList();

            return ServiceResult<List<CourseViewModel>>.Success(categoriesViewModel);
        }


        public async Task<ServiceResult> CreateCourseAsync(CreateCourseModel model)
        {
            string newCreatedFilepath = string.Empty;

            if (model.PictureFormFile is not null && model.PictureFormFile.Length > 0)
            {
                await using var stream = model.PictureFormFile.OpenReadStream();
                var streamPart = new StreamPart(stream, model.PictureFormFile.FileName,
                    model.PictureFormFile.ContentType);


                var fileAsResult = await fileService.UploadFile(streamPart);

                if (fileAsResult.IsFail)
                {
                    //TODO: logging
                    logger.LogInformation("");
                }

                newCreatedFilepath = fileAsResult.Data!.FilePath;
            }


            var createCourseRequest = new CreateCourseRequest(model.Name, model.Description, model.Price,
                newCreatedFilepath, model.CategoryId);

            var response = await catalogService.CreateCourseAsync(createCourseRequest);


            if (!response.IsSuccessStatusCode)

            {
                logger.LogError(response.Error.InnerException, "Course could not be created.");

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ServiceResult.Fail(response.Error.Content!);
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ServiceResult.Fail("A system error occurred. Please try again later.");
                }
            }


            return response.Content!;
        }


        public async Task<ServiceResult<List<CategoryModel>>> GetCategoryList()
        {
            var response = await catalogService.GetCategoriesAsync();


            if (!response.IsSuccessStatusCode)
            {
                return ServiceResult<List<CategoryModel>>.Fail(response.Error.Content!);
            }


            var categoryModelList = response.Content!.Data!.Select(x => new CategoryModel(x.Id, x.Name)).ToList();
            return ServiceResult<List<CategoryModel>>.Success(categoryModelList);
        }
    }
}