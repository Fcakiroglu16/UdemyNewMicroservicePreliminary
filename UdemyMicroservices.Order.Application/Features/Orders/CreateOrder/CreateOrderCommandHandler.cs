using System.Net;
using MediatR;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Domain.Entities;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //create adress
        var address = new Address(request.Address.Line, request.Address.Province,
            request.Address.District, request.Address.ZipCode);

        var order = new Domain.Entities.Order(request.BuyerId, request.DiscountRate, address);


        foreach (var item in request.OrderItems)
            order.AddOrderItem(new OrderItem(item.ProductId, item.ProductName, item.UnitPrice,
                request.DiscountRate));

        if (order.GetTotalItems() == 0)
            return ServiceResult<Guid>.Error("Order item not found.", "Order must have at least one item.",
                HttpStatusCode.BadRequest);


        await orderRepository.AddAsync(order);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult<Guid>.SuccessAsCreated(order.Id, "");
    }
}