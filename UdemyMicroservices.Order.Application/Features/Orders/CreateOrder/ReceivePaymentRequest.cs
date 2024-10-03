using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder
{
    public record ReceivePaymentRequest(
        string OrderCode,
        string CardNumber,
        string CardHolderName,
        string ExpiryDate,
        string CVV,
        decimal Amount);
}