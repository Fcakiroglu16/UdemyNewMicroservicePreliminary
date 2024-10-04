using System.Net;
using UdemyMicroservices.Shared.Extensions;
using UdemyMicroservices.Web.Pages.Order.Dto;
using UdemyMicroservices.Web.Pages.Order.ViewModel;
using UdemyMicroservices.Web.Services.Refit;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services
{
    public class OrderService(IOrderService orderService, ILogger<OrderService> logger)
    {
        public async Task<ServiceResult> CreateOrder(CreateOrderViewModel viewModel)
        {
            //createAddressDto
            var address = new AddressDto(viewModel.Address.Province, viewModel.Address.District,
                viewModel.Address.Line, viewModel.Address.ZipCode);

            //paymentDto
            var payment = new PaymentDto(viewModel.Payment.CardNumber, viewModel.Payment.CardHolderName,
                viewModel.Payment.ExpiryDate, viewModel.Payment.Cvv, viewModel.TotalPrice);


            // orderItems
            var orderItems = viewModel.OrderItems.Select(x => new OrderItemDto(x.ProductId, x.ProductName, x.UnitPrice))
                .ToList();


            var createOrderRequest = new CreateOrderRequest(viewModel.DiscountRate, address, payment, orderItems);


            var response = await orderService.CreateOrder(createOrderRequest);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ServiceResult.FailFromProblemDetails(response.Error);
                }

                logger.LogProblemDetails(response.Error);
                return ServiceResult.Fail("An error occurred while creating the order");
            }

            return ServiceResult.Success();
        }
    }
}