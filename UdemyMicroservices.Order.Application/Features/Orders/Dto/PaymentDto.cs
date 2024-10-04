namespace UdemyMicroservices.Order.Application.Features.Orders.Dto;

public record PaymentDto(
    string CardNumber,
    string CardHolderName,
    string ExpiryDate,
    string Cvv,
    decimal Amount);