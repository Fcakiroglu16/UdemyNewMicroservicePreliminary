using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.Application.Features.Orders.GetOrderHistory;

public record GetOrderHistoryQuery : IRequestByServiceResult<List<GetOrderHistoryResponse>>;