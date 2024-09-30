using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate;

public record ApplyDiscountCouponCommand(string Coupon, float Rate) : IRequestByServiceResult;