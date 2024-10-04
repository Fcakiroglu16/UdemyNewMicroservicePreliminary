using MassTransit;

namespace UdemyMicroservices.Payment.Repositories;

public class Payment
{
    private Payment()
    {
    }

    public Payment(Guid userId, string orderCode, decimal amount)
    {
        CreateNew(userId, orderCode, amount);
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; } = default!;
    public string OrderCode { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;
    public PaymentStatus Status { get; private set; }

    public string? Error { get; private set; }

    // Factory method for creating a new payment
    public void CreateNew(Guid userId, string orderCode, decimal amount)
    {
        ArgumentException.ThrowIfNullOrEmpty(orderCode);

        if (amount <= 0) throw new InvalidOperationException("Payment amount must be greater than zero.");


        //if (string.IsNullOrWhiteSpace(userId))
        //{
        //    throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        //}

        //if (string.IsNullOrWhiteSpace(orderCode))
        //{
        //    throw new ArgumentException("Order code cannot be empty.", nameof(orderCode));
        //}

        Id = NewId.NextGuid();
        UserId = userId;
        OrderCode = orderCode;
        Amount = amount;
        Status = PaymentStatus.Pending;
    }

    public void SetSuccessStatus()
    {
        Status = PaymentStatus.Success;
    }

    public void SetFailStatus(string error)
    {
        Status = PaymentStatus.Fail;
        Error = error;
    }
}

public enum PaymentStatus
{
    Pending = 0,
    Success = 1,
    Fail = 2
}