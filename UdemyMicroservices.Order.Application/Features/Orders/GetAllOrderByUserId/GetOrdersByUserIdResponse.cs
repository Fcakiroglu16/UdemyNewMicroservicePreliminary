using UdemyMicroservices.Order.Application.Features.Orders.Dto;

namespace UdemyMicroservices.Order.Application.Features.Orders.GetAllOrderByUserId
{
    public record GetOrdersByUserIdResponse(DateTime OrderDate, decimal TotalPrice, List<OrderItemDto> OrderItems);
}