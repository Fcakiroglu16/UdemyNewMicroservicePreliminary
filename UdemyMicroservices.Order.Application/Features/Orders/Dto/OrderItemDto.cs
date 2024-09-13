namespace UdemyMicroservices.Order.Application.Features.Orders.Dto;

public record OrderItemDto(
    string ProductId,
    string ProductName,
    decimal UnitPrice
);