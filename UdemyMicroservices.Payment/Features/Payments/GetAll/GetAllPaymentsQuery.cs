using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Payment.Features.Payments.GetAll;

public record GetAllPaymentsQuery : IRequestByServiceResult<List<PaymentsDto>>;