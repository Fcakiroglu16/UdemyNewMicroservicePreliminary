using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Shared.Options;

public class BusOption
{
    public BusOptionItem RabbitMq { get; set; } = default!;
}

public class BusOptionItem
{
    [Required] public string Address { get; set; } = default!;

    [Required] public int Port { get; set; }

    [Required] public string UserName { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
}