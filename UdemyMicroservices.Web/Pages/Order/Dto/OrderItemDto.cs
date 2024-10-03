namespace UdemyMicroservices.Web.Pages.Order.Dto;

public record OrderItemViewModel(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice
);