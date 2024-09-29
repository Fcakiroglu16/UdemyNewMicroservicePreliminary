namespace UdemyMicroservices.Web.Pages.Basket.Dto
{
    public record BasketResponse(
        string UserId,
        string? Coupon,
        float? DiscountRate,
        List<BasketItemDto> BasketItems)
    {
        public decimal TotalPrice => BasketItems.Sum(x => x.CoursePrice);

        public decimal? TotalPriceByApplyDiscountRate => BasketItems.Sum(x => x.CoursePriceByApplyDiscountRate);
    }
}