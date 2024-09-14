using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Discount.Repositories;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Discount.Features.Discount.GetDiscountByCode;

public class GetDiscountByCodeQueryHandler(AppDbContext context, IIdentityService identityService)
    : IRequestHandler<GetDiscountByCodeQuery, ServiceResult<GetDiscountByCodeQueryResponse>>
{
    public async Task<ServiceResult<GetDiscountByCodeQueryResponse>> Handle(GetDiscountByCodeQuery request,
        CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId;

        var discount =
            await context.Discounts.FirstOrDefaultAsync(c => c.Code == request.DiscountCode && c.UserId == userId,
                cancellationToken);

        if (discount is null)
            return ServiceResult<GetDiscountByCodeQueryResponse>.Error("Discount Not Found",
                $"The discount with code '{request.DiscountCode}' was not found.", HttpStatusCode.NotFound);


        if (discount.Expired < DateTime.Now)
            return ServiceResult<GetDiscountByCodeQueryResponse>.Error("Discount Expired",
                $"The discount with code '{request.DiscountCode}' was expired.", HttpStatusCode.BadRequest);


        return ServiceResult<GetDiscountByCodeQueryResponse>.SuccessAsOk(
            new GetDiscountByCodeQueryResponse(discount.Rate));
    }
}