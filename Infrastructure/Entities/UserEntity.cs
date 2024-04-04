using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set;} = null!;

    public string? Bio {  get; set; }

    public string? ImageUrl { get; set; } = "cat.jpg";

    public int? AddressId { get; set; }

    public AddressEntity? Address { get; set; }

    public bool IsExternalAccount { get; set; }
}
