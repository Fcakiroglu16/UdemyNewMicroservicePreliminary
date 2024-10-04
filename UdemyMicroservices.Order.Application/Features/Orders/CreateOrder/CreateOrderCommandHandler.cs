using System.Net;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refit;
using UdemyMicroservices.Bus;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Application.Contracts.refit;
using UdemyMicroservices.Order.Application.Features.Orders.Dto;
using UdemyMicroservices.Order.Domain.Entities;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Extensions;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IIdentityService identityService,
    IPaymentService paymentService,
    ILogger<CreateOrderCommandHandler> logger,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateOrderCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //TODO : must flag coupon as used (via bus) after order created successfully
        //TODO : must empty basket after order created (via bus)
        //TODO : must use Two Phase Commit (2PC) or Saga Pattern
        var order = CreateOrder(request);

        if (order.GetTotalItems() == 0)
            return ServiceResult<Guid>.Error("Order item not found.", "Order must have at least one item.",
                HttpStatusCode.BadRequest);

        await SaveOrderAsync(order);

        var paymentResult = await ProcessPaymentAsync(order, request.Payment);

        if (!paymentResult.IsSuccessStatusCode) return HandlePaymentFailure(paymentResult);


        // TODO : send message to bus
        // TODO : outbox pattern


        return await UpdateOrderStatusAsync(order, paymentResult);
    }

    private Domain.Entities.Order CreateOrder(CreateOrderCommand request)
    {
        var address = new Address(request.Address.Line, request.Address.Province,
            request.Address.District, request.Address.ZipCode);

        var order = Domain.Entities.Order.CreateUnPaidOrder(identityService.GetUserId, request.DiscountRate, address);

        foreach (var item in request.OrderItems)
            order.AddOrderItem(new OrderItem(item.ProductId, item.ProductName, item.UnitPrice));

        return order;
    }

    private async Task SaveOrderAsync(Domain.Entities.Order order)
    {
        await orderRepository.AddAsync(order);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<ApiResponse<ServiceResult<ReceivePaymentResponse>>> ProcessPaymentAsync(
        Domain.Entities.Order order,
        PaymentDto payment)
    {
        var paymentRequest = new ReceivePaymentRequest(order.OrderCode, payment.CardNumber,
            payment.CardHolderName, payment.ExpiryDate, payment.Cvv, order.TotalPrice);

        return await paymentService.ReceivePaymentAsync(paymentRequest);
    }

    private ServiceResult<ReceivePaymentResponse> HandlePaymentFailure(
        ApiResponse<ServiceResult<ReceivePaymentResponse>> paymentResult)
    {
        if (paymentResult.StatusCode == HttpStatusCode.BadRequest)
            return ServiceResult<ReceivePaymentResponse>.ErrorFromProblemDetails(paymentResult.Error);


        logger.LogProblemDetails(paymentResult.Error);
        return ServiceResult<ReceivePaymentResponse>.Error("Payment service is not available.",
            "Payment service is not available.",
            HttpStatusCode.ServiceUnavailable);
    }

    private async Task<ServiceResult> UpdateOrderStatusAsync(Domain.Entities.Order order,
        ApiResponse<ServiceResult<ReceivePaymentResponse>> paymentResult)
    {
        try
        {
            order.SetPaidStatus(paymentResult.Content!.Data.PaymentId);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogCritical(e,
                "An error occurred while updating the order status. PaymentId={PaymentId},OrderCode={OrderCode}",
                paymentResult.Content!.Data, order.OrderCode);
        }


        await publishEndpoint.Publish(new OrderCreatedEvent(identityService.GetUserId, order.OrderCode,
            order.TotalPrice,
            order.OrderDate));


        return ServiceResult.SuccessAsNoContent();
    }
}