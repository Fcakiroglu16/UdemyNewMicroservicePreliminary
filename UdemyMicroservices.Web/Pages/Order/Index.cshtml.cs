using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyMicroservices.Web.Pages.Order.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Order
{
    public class IndexModel(BasketService basketService, OrderService orderService) : BasePageModel
    {
        [BindProperty] public OrderViewModel Order { get; set; } = OrderViewModel.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var basketAsResult = await basketService.GetBasketsAsync();


            if (basketAsResult.IsFail) return ErrorPage(basketAsResult);


            foreach (var basketItem in basketAsResult.Data!.BasketItems)
            {
                Order.AddOrderItem(basketItem, basketAsResult.Data.DiscountRate);
            }

            Order.TotalPrice = basketAsResult.Data.CurrentTotalPrice();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var basketAsResult = await basketService.GetBasketsAsync();


            Order.TotalPrice = basketAsResult.Data!.CurrentTotalPrice();
            Order.DiscountRate = basketAsResult.Data.DiscountRate;
            foreach (var basketItem in basketAsResult.Data!.BasketItems)
            {
                Order.AddOrderItem(basketItem, basketAsResult.Data.DiscountRate);
            }

            var result = await orderService.CreateOrder(Order);

            return result.IsFail
                ? ErrorPage(result, "/Order/Index")
                : SuccessPage("order created successfully", "Index");
        }
    }
}