using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Order.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Order;

public class CreateModel(BasketService basketService, OrderService orderService) : BasePageModel
{
    [BindProperty] public CreateOrderViewModel CreateOrderViewModel { get; set; } = CreateOrderViewModel.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadInitialFormData();

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        await LoadInitialFormData();
        if (!ModelState.IsValid) return Page();


        var result = await orderService.CreateOrder(CreateOrderViewModel);

        return result.IsFail
            ? ErrorPage(result)
            : SuccessPage("order created successfully", "/Order/Result");
    }

    private async Task LoadInitialFormData()
    {
        var basketAsResult = await basketService.GetBasketsAsync();
        CreateOrderViewModel.TotalPrice = basketAsResult.Data!.CurrentTotalPrice;
        CreateOrderViewModel.DiscountRate = basketAsResult.Data.DiscountRate;
        foreach (var basketItem in basketAsResult.Data!.BasketItems) CreateOrderViewModel.AddOrderItem(basketItem);
    }
}