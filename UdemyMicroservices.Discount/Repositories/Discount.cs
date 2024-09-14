namespace UdemyMicroservices.Discount.Repositories;

public class Discount
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public float Rate { get; set; }
    public string Code { get; set; } = default;
    public DateTime Created { get; set; }

    public DateTime Expired { get; set; }
}