using System.Collections.Immutable;

namespace UdemyMicroservices.Web.Pages.Order.ViewModel;

public record OrderHistoryViewModel(string DateTime, string TotalPrice)
{
    private List<OrderItemViewModel> OrderItems { get; } = new();

    public ImmutableList<OrderItemViewModel> GetItems => OrderItems.ToImmutableList();


    public void AddItem(Guid productId, string productName, decimal unitPrice)
    {
        OrderItems.Add(new OrderItemViewModel(productId, productName, unitPrice));
    }
}