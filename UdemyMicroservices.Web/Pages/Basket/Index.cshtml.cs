using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.Pages.Basket.ViewModel;
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
            Basket.Items.Add(new BasketViewModelItem(basketItem.Id, basketItem.PictureUrl,
                basketItem.Name,
                basketItem.Price, basketItem.PriceByApplyDiscountRate));

        return Page();
    }


    public async Task<IActionResult> OnGetAddBasketAsync(Guid courseId)
    {
        var course = await catalogService.GetCourse(courseId);


        //var basketItem = new BasketItemDto(course.Data!.Id, course.Data.Name, course.Data.PictureUrl,
        //    course.Data.Price, null,);


        var createOrUpdateBasket = new AddBasketRequest(course.Data!.Id, course.Data.Name,
            course.Data.PictureUrl, course.Data.Price);


        var result = await basketService.CreateOrUpdateBasketAsync(createOrUpdateBasket);

        return result.IsFail ? ErrorPage(result, "Index") : SuccessPage("course added to basket", "Index");
    }

    public async Task<IActionResult> OnGetDeleteAsync(Guid courseId)
    {
        var result = await basketService.DeleteBasketAsync(courseId);

        return result.IsFail ? ErrorPage(result, "Index") : SuccessPage("course deleted from basket", "Index");
    }

    public async Task<IActionResult> OnPostApplyDiscountAsync(string couponCode)
    {
        var response = await basketService.ApplyDiscountAsync(couponCode);

        return response.IsFail ? ErrorPage(response, "Index") : SuccessPage("discount coupon applied", "Index");
    }

    public async Task<IActionResult> OnGetRemoveDiscountAsync()
    {
        var response = await basketService.RemoveDiscountAsync();

        return response.IsFail ? ErrorPage(response, "Index") : SuccessPage("discount coupon removed", "Index");
    }
}