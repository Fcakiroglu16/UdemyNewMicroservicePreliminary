using MediatR;
using System.Net;
using UdemyMicroservices.Payment.Repositories;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Payment.Features.Payments.Receive
{
    public class ReceivePaymentCommandHandler(AppDbContext context, IIdentityService identityService)
        : IRequestHandler<ReceivePaymentCommand, ServiceResult<ReceivePaymentResponse>>
    {
        public async Task<ServiceResult<ReceivePaymentResponse>> Handle(ReceivePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var newPayment = new Repositories.Payment(identityService.GetUserId, request.OrderCode, request.Amount);

            var processResult = await ExternalPaymentProcessAsync(request.CardNumber, request.CardHolderName,
                request.ExpiryDate,
                request.CVV,
                request.Amount);

            await context.Payments.AddAsync(newPayment, cancellationToken);

            if (!processResult)
            {
                newPayment.SetFailStatus("Insufficient Credit Limit");
                await context.SaveChangesAsync(cancellationToken);

                return ServiceResult<ReceivePaymentResponse>.Error("Insufficient Credit Limit",
                    $"Your credit card has insufficient funds to complete this transaction. (Amount = {request.Amount})",
                    HttpStatusCode.BadRequest);
            }

            newPayment.SetSuccessStatus();
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult<ReceivePaymentResponse>.SuccessAsOk(new ReceivePaymentResponse(newPayment.Id));
        }

        private async Task<bool> ExternalPaymentProcessAsync(string cardNumber, string cardHolderName,
            string expiryDate, string cvv,
            decimal amount)
        {
            // Perform the external payment process asynchronously
            await Task.Delay(100); // Simulating an asynchronous operation

            return true;
        }
    }
}