namespace UdemyMicroservices.Order.Domain.Entities;

public class Address : BaseEntity<int>
{
    private Address()
    {
    }


    public Address(string line, string province, string district, string? zipCode = null)
    {
        SetAddress(line, province, district, zipCode);
    }

    public string Line { get; private set; } = default!;
    public string Province { get; private set; } = default!;

    public string District { get; private set; } = default!;


    public string? ZipCode { get; private set; }

    public Order Order { get; set; } = default!;
    public Guid OrderId { get; set; }

    // Business method to set or update the address
    public void SetAddress(string line, string province, string district, string? zipCode = null)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentException("Address line cannot be empty.");

        if (string.IsNullOrWhiteSpace(province))
            throw new ArgumentException("Province cannot be empty.");

        if (string.IsNullOrWhiteSpace(district))
            throw new ArgumentException("District cannot be empty.");

        //zip code check
        if (!string.IsNullOrWhiteSpace(zipCode) && zipCode.Length > 10)
            throw new ArgumentException("ZipCode cannot be longer than 10 characters.");


        Line = line;
        Province = province;
        District = district;
        ZipCode = zipCode; // ZipCode is optional, can be null or empty
    }


    // Business method to update ZipCode
    public void UpdateZipCode(string? zipCode)
    {
        // You can add validation logic here based on the country or region rules
        if (!string.IsNullOrWhiteSpace(zipCode) && zipCode.Length > 10)
            throw new ArgumentException("ZipCode cannot be longer than 10 characters.");

        ZipCode = zipCode;
    }

    // Business method to format the full address as a single string
    public string GetFullAddress()
    {
        var fullAddress = $"{Line}, {District}, {Province}";
        if (!string.IsNullOrWhiteSpace(ZipCode)) fullAddress += $", {ZipCode}";

        return fullAddress;
    }

    // Business method to check if two addresses are the same
    public bool IsSameAs(Address otherAddress)
    {
        if (otherAddress == null)
            throw new ArgumentNullException(nameof(otherAddress));

        return Line == otherAddress.Line &&
               Province == otherAddress.Province &&
               District == otherAddress.District &&
               ZipCode == otherAddress.ZipCode;
    }
}