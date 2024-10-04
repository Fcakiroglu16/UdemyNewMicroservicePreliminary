using System.Text;
using MassTransit;

namespace UdemyMicroservices.Order.Domain.Entities;

public class Order : BaseEntity<Guid>
{
    public string OrderCode { get; private set; } = default!;

    public DateTime OrderDate { get; private set; }
    public Guid BuyerId { get; private set; }

    public decimal TotalPrice { get; private set; }

    public float? DiscountRate { get; private set; }
    public ICollection<OrderItem> OrderItems { get; } = [];
    public Address Address { get; set; } = default!;

    public OrderStatus Status { get; set; }

    public Guid? PaymentId { get; set; }

    private static string GenerateOrderCode()
    {
        var random = new Random();
        var orderCode = new StringBuilder(10); // Başlangıç kapasitesi 10 olarak ayarlanır.

        for (var i = 0; i < 10; i++) orderCode.Append(random.Next(0, 10)); // Sayıları doğrudan ekler.

        return orderCode.ToString(); // Sonuç olarak string döndürülür.
    }

    public static Order CreateUnPaidOrder(Guid buyerId, float? discountRate, Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (discountRate is < 0 or > 100)
            throw new ArgumentException("Discount rate must be between 0 and 100.");

        return new Order
        {
            BuyerId = buyerId,
            Id = NewId.NextGuid(),
            OrderDate = DateTime.Now,
            DiscountRate = discountRate,
            Address = address,
            OrderCode = GenerateOrderCode(),
            Status = OrderStatus.WaitingForPayment
        };
    }

    // Business method to add an OrderItem
    public void AddOrderItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        OrderItems.Add(item);
        //if (item == null)
        //    throw new ArgumentException("Order item cannot be null.");

        //OrderItems.Add(item);
        UpdateTotalPrice();
    }

    // Business method to remove an OrderItem
    public void RemoveOrderItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        OrderItems.Remove(item);
        UpdateTotalPrice();
    }

    // Business method to calculate the total price of the order
    private void UpdateTotalPrice()
    {
        TotalPrice = OrderItems.Sum(item => item.UnitPrice); // Assuming quantity 1 for now, can be extended.
    }

    // Business method to check if an order can still be modified
    public bool CanModifyOrder()
    {
        return (DateTime.Now - OrderDate).TotalHours < 24; // Example: orders can only be modified within 24 hours
    }


    // Business method to check if the order has a specific product
    public bool HasProduct(Guid productId)
    {
        return OrderItems.Any(item => item.ProductId == productId);
    }


    // Business method to retrieve the total number of items in the order
    public int GetTotalItems()
    {
        return OrderItems.Count;
    }

    public void SetPaidStatus(Guid paymentId)
    {
        PaymentId = paymentId;
        Status = OrderStatus.Paid;
    }
}

public enum OrderStatus
{
    WaitingForPayment,
    Paid,
    Cancelled
}