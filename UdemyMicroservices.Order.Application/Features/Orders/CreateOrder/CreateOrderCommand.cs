using UdemyMicroservices.Order.Application.Features.Orders.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    string BuyerId,
    float? DiscountRate,
    AddressDto Address,
    List<OrderItemDto> OrderItems)
    : IRequestByServiceResult<Guid>;