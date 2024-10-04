namespace UdemyMicroservices.Basket.Dto;

public record BasketDto(Guid UserId, List<BasketItemDto> BasketItems)
{
    public float? DiscountRate { get; set; }
    public string? Coupon { get; set; }


    public bool IsApplyDiscountRate => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);

    public decimal TotalPrice => BasketItems.Sum(x => x.Price);

    public decimal? TotalPriceByApplyDiscountRate => BasketItems.Sum(x => x.PriceByApplyDiscountRate);

    public decimal CurrentTotalPrice => DiscountRate is not null ? TotalPriceByApplyDiscountRate!.Value : TotalPrice;


    public void ApplyNewDiscount(string coupon, float rate)
    {
        DiscountRate = rate;
        Coupon = coupon;
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            var newPriceWithApplyDiscountRate = item.Price * (decimal)(1 - rate);
            BasketItems[i] = item with { PriceByApplyDiscountRate = newPriceWithApplyDiscountRate };
        }
    }

    public void ApplyAvailableDiscount()
    {
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            var newPriceWithApplyDiscountRate = item.Price * (decimal)(1 - DiscountRate!);
            BasketItems[i] = item with { PriceByApplyDiscountRate = newPriceWithApplyDiscountRate };
        }
    }


    public void RemoveDiscount()
    {
        DiscountRate = null;
        Coupon = null;
        for (var i = 0; i < BasketItems.Count; i++)
        {
            var item = BasketItems[i];
            BasketItems[i] = item with { PriceByApplyDiscountRate = null };
        }
    }
}