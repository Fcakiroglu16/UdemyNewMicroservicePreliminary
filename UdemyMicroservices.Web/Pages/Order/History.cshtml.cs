using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Order.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Order;

[Authorize]
public class HistoryModel(OrderService orderService) : BasePageModel
{
    public List<OrderHistoryViewModel> OrderHistoryList { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await orderService.GetHistory();


        if (response.IsFail) return ErrorPage(response);

        OrderHistoryList = response.Data!;


        return Page();
    }
}