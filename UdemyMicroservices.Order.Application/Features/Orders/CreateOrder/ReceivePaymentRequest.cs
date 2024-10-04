namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public record ReceivePaymentRequest(
    string OrderCode,
    string CardNumber,
    string CardHolderName,
    string ExpiryDate,
    string CVV,
    decimal Amount);