using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Payment.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Payment.Features.Payments.GetAll
{
    public class GetAllPaymentsQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetAllPaymentsQuery, ServiceResult<List<PaymentsDto>>>
    {
        public async Task<ServiceResult<List<PaymentsDto>>> Handle(GetAllPaymentsQuery request,
            CancellationToken cancellationToken)
        {
            var payments = await context.Payments.OrderByDescending(x => x.PaymentDate)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = mapper.Map<List<PaymentsDto>>(payments);

            return ServiceResult<List<PaymentsDto>>.SuccessAsOk(result);
        }
    }
}