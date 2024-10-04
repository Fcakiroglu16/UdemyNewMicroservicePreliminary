namespace UdemyMicroservices.Order.Domain.Entities;

public class OrderItem : BaseEntity<int>
{
    private OrderItem()
    {
    }

    public OrderItem(Guid productId, string productName, decimal productPrice)
    {
        SetItem(productId, productName, productPrice);
    }
    //// Constructor to initialize an OrderItem
    //public OrderItem(string productId, string productName, decimal productPrice)
    //{
    //    SetItem(productId, productName, productPrice);
    //}

    // Business method to set or update product details


    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public void SetItem(Guid productId, string productName, decimal productPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("ProductName cannot be empty.");

        if (productPrice <= 0)
            throw new ArgumentException("ProductPrice must be greater than zero.");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = productPrice;
    }


    // Business method to update the price of the product
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("New price must be greater than zero.");

        UnitPrice = newPrice;
    }

    // Business method to apply a discount to the product price
    public void ApplyDiscount(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100.");

        var discountAmount = UnitPrice * (discountPercentage / 100);
        UnitPrice -= discountAmount;
    }


    // Business method to check if the OrderItem belongs to the same product
    public bool IsSameItem(OrderItem otherItem)
    {
        if (otherItem == null)
            throw new ArgumentNullException(nameof(otherItem));

        return ProductId == otherItem.ProductId;
    }
}