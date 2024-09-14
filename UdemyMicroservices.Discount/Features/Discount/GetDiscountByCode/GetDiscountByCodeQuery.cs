using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Discount.Features.Discount.GetDiscountByCode;

public record GetDiscountByCodeQuery(string DiscountCode) : IRequestByServiceResult<GetDiscountByCodeQueryResponse>;