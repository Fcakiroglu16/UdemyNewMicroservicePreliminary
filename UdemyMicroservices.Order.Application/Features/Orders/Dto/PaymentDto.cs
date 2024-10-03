using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyMicroservices.Order.Application.Features.Orders.Dto
{
    public record PaymentDto(
        string CardNumber,
        string CardHolderName,
        string ExpiryDate,
        string Cvv,
        decimal Amount);
}