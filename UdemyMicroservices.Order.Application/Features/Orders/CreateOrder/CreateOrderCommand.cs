using UdemyMicroservices.Order.Application.Features.Orders.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    float? DiscountRate,
    AddressDto Address,
    PaymentDto Payment,
    List<OrderItemDto> OrderItems)
    : IRequestByServiceResult;