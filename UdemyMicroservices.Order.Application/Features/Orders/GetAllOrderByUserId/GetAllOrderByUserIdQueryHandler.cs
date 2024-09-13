using AutoMapper;
using MediatR;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Order.Application.Features.Orders.GetAllOrderByUserId;

public class GetAllOrderByUserIdQueryHandler(
    IOrderRepository orderRepository,
    IIdentityService identityService,
    IMapper mapper)
    : IRequestHandler<GetAllOrderByUserIdQuery, ServiceResult<List<GetOrdersByUserIdResponse>>>
{
    public async Task<ServiceResult<List<GetOrdersByUserIdResponse>>> Handle(GetAllOrderByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetOrdersByUserIdAsync(identityService.GetUserId);

        return ServiceResult<List<GetOrdersByUserIdResponse>>.SuccessAsOk(
            mapper.Map<List<GetOrdersByUserIdResponse>>(orders));
    }
}