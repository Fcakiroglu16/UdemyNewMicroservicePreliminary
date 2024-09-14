using AutoMapper;
using UdemyMicroservices.Payment.Features.Payments.GetAll;

namespace UdemyMicroservices.Payment.Features.Payments
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Repositories.Payment, PaymentsDto>();
        }
    }
}