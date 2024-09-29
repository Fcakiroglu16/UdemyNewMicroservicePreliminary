using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Basket;

[Authorize]
public class IndexModel(CatalogService catalogService, BasketService basketService) : BasePageModel
{
    public BasketViewModel Basket { get; set; } = new();


    public async Task<IActionResult> OnGet()
    {
        var basketsAsResult = await basketService.GetBasketsAsync();

        if (basketsAsResult.IsFail) return ErrorPage(basketsAsResult);


        Basket.SetPrice(basketsAsResult.Data!.TotalPrice, basketsAsResult.Data.TotalPriceByApplyDiscountRate);
        Basket.DiscountRate = basketsAsResult.Data.DiscountRate;
        Basket.Coupon = basketsAsResult.Data.Coupon;


        foreach (var basketItem in basketsAsResult.Data!.BasketItems)
            Basket.Items.Add(new BasketViewModelItem(basketItem.CourseId, basketItem.CoursePictureUrl,
                basketItem.CourseName,
                basketItem.CoursePrice, basketItem.CoursePriceByApplyDiscountRate));

        return Page();
    }


    public async Task<IActionResult> OnGetAddBasketAsync(Guid courseId)
    {
        var course = await catalogService.GetCourse(courseId);


        var basketItem = new BasketItemDto(course.Data!.Id, course.Data.Name, course.Data.PictureUrl,
            course.Data.Price, null);


        var createOrUpdateBasket = new AddBasketRequest(basketItem.CourseId, basketItem.CourseName,
            basketItem.CoursePictureUrl, basketItem.CoursePrice);


        var result = await basketService.CreateOrUpdateBasketAsync(createOrUpdateBasket);

        if (result.IsFail) return ErrorPage(result);


        return RedirectToPage("index");
    }

    public async Task<IActionResult> OnGetDeleteAsync(Guid courseId)
    {
        var result = await basketService.DeleteBasketAsync(courseId);

        if (result.IsFail) return ErrorPage(result);

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostApplyDiscountAsync(string couponCode)
    {
        var response = await basketService.ApplyDiscountAsync(couponCode);

        if (response.IsFail)
        {
            var basketsAsResult = await basketService.GetBasketsAsync();

            if (basketsAsResult.IsFail) return ErrorPage(basketsAsResult);


            foreach (var basketItem in basketsAsResult.Data!.BasketItems)
                Basket.Items.Add(new BasketViewModelItem(basketItem.CourseId, basketItem.CoursePictureUrl,
                    basketItem.CourseName,
                    basketItem.CoursePrice, basketItem.CoursePriceByApplyDiscountRate));

            return ErrorPage(response);
        }

        return RedirectToPage("Index");
    }
}