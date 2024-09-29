using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(Guid CourseId) : IRequestByServiceResult;