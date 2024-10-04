using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.Pages.Order.Dto;

namespace UdemyMicroservices.Web.Pages.Order.ViewModel
{
    public record CreateOrderViewModel
    {
        public AddressViewModel Address { get; set; } = default!;

        public PaymentViewModel Payment { get; set; } = default!;

        [ValidateNever] public List<OrderItemViewModel> OrderItems { get; set; } = [];


        [ValidateNever] public float? DiscountRate { get; set; }


        public decimal TotalPrice { get; set; }

        public static CreateOrderViewModel Empty => new CreateOrderViewModel
        {
            Address = AddressViewModel.Empty,
            Payment = PaymentViewModel.Empty
        };


        public void AddOrderItem(BasketItemDto basketItem, float? discountRate)
        {
            if (discountRate is not null)
            {
                OrderItems.Add(new(basketItem.CourseId, basketItem.CourseName,
                    basketItem.CoursePriceByApplyDiscountRate!.Value));
            }
            else
            {
                OrderItems.Add(new(basketItem.CourseId, basketItem.CourseName,
                    basketItem.CoursePrice));
            }
        }
    }
}