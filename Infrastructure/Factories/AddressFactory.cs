using Infrastructure.Entities;
using Infrastructure.Models.Account;
using Infrastructure.Repositories;

namespace Infrastructure.Factories;

public class AddressFactory(AddressRepository addressRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;

    public async Task<AccountDetailsAddressFormModel> PopulateAddressForm(UserEntity userEntity)
    {
        var result = await _addressRepository.GetOneAsync(x => x.Id == userEntity.AddressId);
        var addressEntity = (AddressEntity)result.ContentResult!;
        var model = new AccountDetailsAddressFormModel();

        if(addressEntity != null)
        {
            model.AddressLine1 = addressEntity.Addressline_1;
            model.AddressLine2 = addressEntity.Addressline_2;
            model.PostalCode = addressEntity.PostalCode;
            model.City = addressEntity.City;
        }
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
