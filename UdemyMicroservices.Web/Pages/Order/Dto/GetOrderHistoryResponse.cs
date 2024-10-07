using UdemyMicroservices.Web.Pages.Order.ViewModel;

namespace UdemyMicroservices.Web.Pages.Order.Dto;

public record GetOrderHistoryResponse(DateTime OrderDate, decimal TotalPrice, List<OrderItemViewModel> OrderItems);