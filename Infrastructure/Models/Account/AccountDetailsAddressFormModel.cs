using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class AccountDetailsAddressFormModel
{
    [Display(Name = "Address Line 1", Prompt = "Enter your address line", Order = 0)]
    [Required(ErrorMessage = "Address line 1 is required")]
    public string AddressLine1 { get; set; } = null!;

    [Display(Name = "Address Line 2", Prompt = "Enter your secondary address line", Order = 1)]
    public string? AddressLine2 { get; set; }

    [Display(Name = "Postal Code", Prompt = "Enter your postal code", Order = 2)]
    [Required(ErrorMessage = "Postal code is required")]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter your city", Order = 3)]
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = null!;
}
