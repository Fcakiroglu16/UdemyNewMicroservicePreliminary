using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Pages.Order.ViewModel;

public record AddressViewModel
{
    [Display(Name = "Address Line")] public string Line { get; set; } = default!;

    [Display(Name = "Province")] public string Province { get; set; } = default!;

    [Display(Name = "District")] public string District { get; set; } = default!;

    [Display(Name = "Zip Code")] public string ZipCode { get; set; } = default!;

    public static AddressViewModel Empty => new();
}