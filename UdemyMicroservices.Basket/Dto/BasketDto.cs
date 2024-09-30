namespace UdemyMicroservices.Basket.Dto;

public record BasketDto(string UserId, List<BasketItemDto> BasketItems)
{
    public float? DiscountRate { get; set; }
    public string? Coupon { get; set; }


    public bool IsApplyDiscountRate => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);


    public void ApplyNewDiscount(string coupon, float rate)
    {
        DiscountRate = rate;
        Coupon = coupon;
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            var newPriceWithApplyDiscountRate = item.CoursePrice * (decimal)(1 - rate);
            BasketItems[i] = item with { CoursePriceByApplyDiscountRate = newPriceWithApplyDiscountRate };
        }
    }

    public void ApplyAvailableDiscount()
    {
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            var newPriceWithApplyDiscountRate = item.CoursePrice * (decimal)(1 - DiscountRate!);
            BasketItems[i] = item with { CoursePriceByApplyDiscountRate = newPriceWithApplyDiscountRate };
        }
    }


    public void RemoveDiscount()
    {
        DiscountRate = null;
        Coupon = null;
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            BasketItems[i] = item with { CoursePriceByApplyDiscountRate = null };
        }
    }
}