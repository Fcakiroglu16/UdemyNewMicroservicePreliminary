using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Shared.Options
{
    public class MicroserviceOption
    {
        public MicroserviceOptionItem? Payment { get; set; }
    }

    public class MicroserviceOptionItem
    {
        [Required] public string Address { get; set; } = default!;
    }
}