namespace UdemyMicroservices.Order.Application.Features.Orders.Dto;

public record OrderDto(
    Guid Id,
    string BuyerId,
    decimal TotalPrice,
    List<OrderItemDto> OrderItems,
    AddressDto Address,
    DateTime OrderDate
);