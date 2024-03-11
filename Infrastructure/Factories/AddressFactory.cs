using Infrastructure.Entities;
using Infrastructure.Models.Account;

namespace Infrastructure.Factories;

public class AddressFactory
{
    public AccountDetailsAddressFormModel PopulateAddressForm(UserEntity userEntity)
    {
        var model = new AccountDetailsAddressFormModel();

        if (userEntity.AddressId != null)
        {
            model.AddressLine1 = userEntity.Address!.Addressline_1;
            model.AddressLine2 = userEntity.Address.Addressline_2;
            model.PostalCode = userEntity.Address.PostalCode;
            model.City = userEntity.Address.City;
        };

        return model;
    }

    public AddressEntity PopulateAddressEntity(AccountDetailsAddressFormModel model)
    {
        var entity = new AddressEntity();

        if (model != null)
        {
            entity.Addressline_1 = model.AddressLine1;
            entity.Addressline_2 = model.AddressLine2;
            entity.City = model.City;
            entity.PostalCode = model.PostalCode;
        }

        return entity;
    }
}
