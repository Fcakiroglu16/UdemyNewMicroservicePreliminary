using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Options
{
    public class BaseServiceOption
    {
        [Required] public string Address { get; set; } = default!;
    }
}