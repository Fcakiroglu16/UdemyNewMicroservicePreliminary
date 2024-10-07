using UdemyMicroservices.Order.Application.Features.Orders.Dto;

namespace UdemyMicroservices.Order.Application.Features.Orders.GetOrderHistory;

public record GetOrderHistoryResponse(DateTime OrderDate, decimal TotalPrice, List<OrderItemDto> OrderItems);