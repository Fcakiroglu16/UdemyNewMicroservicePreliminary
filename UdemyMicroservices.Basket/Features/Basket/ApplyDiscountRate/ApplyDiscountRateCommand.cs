using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate;

public record ApplyDiscountRateCommand(string Coupon, float Rate) : IRequestByServiceResult;