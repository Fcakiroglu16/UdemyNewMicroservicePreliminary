namespace UdemyMicroservices.Bus;

public record OrderCreatedEvent(Guid UserId, string OrderCode, decimal TotalPrice, DateTime CreatedDate);