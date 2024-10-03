using System.Globalization;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.Pages.Order.Dto;

namespace UdemyMicroservices.Web.Pages.Order.ViewModel
{
    public record OrderViewModel
    {
        public AddressViewModel Address { get; set; } = default!;
        public List<OrderItemViewModel> OrderItems { get; set; } = [];
        public PaymentViewModel Payment { get; set; } = default!;

        public float? DiscountRate { get; set; }

        public decimal TotalPrice { get; set; }

        public static OrderViewModel Empty => new OrderViewModel
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