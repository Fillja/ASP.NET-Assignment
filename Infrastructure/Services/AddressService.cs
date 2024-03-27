using Infrastructure.Models;
using Infrastructure.Entities;
using Infrastructure.Models.Account;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class AddressService(AddressRepository addressRepository, AddressFactory addressFactory, UserManager<UserEntity> userManager)
{
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly AddressFactory _addressFactory = addressFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<ResponseResult> GetOrCreateAddressAsync(AccountDetailsAddressFormModel model)
    {
        try
        {
            Expression<Func<AddressEntity, bool>> adressExpression = x => x.Addressline_1 == model.AddressLine1 && x.Addressline_2 == model.AddressLine2 && x.City == model.City && x.PostalCode == model.PostalCode;
            var existResult = await _addressRepository.ExistsAsync(adressExpression);

            if (existResult.StatusCode == StatusCode.EXISTS)
            {
                var getResult = await _addressRepository.GetOneAsync(adressExpression);
                if (getResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(getResult.ContentResult!, "Address found successfully.");
            }
            else if (existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var newAdressEntity = _addressFactory.PopulateAddressEntity(model);
                var createResult = await _addressRepository.CreateAsync(newAdressEntity);
                if (createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(createResult.ContentResult!, "Address created succesfully.");
            }

            return ResponseFactory.Error("Something went wrong with creating or getting the address.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> UpdateUserWithAddress(UserEntity userEntity, AccountDetailsAddressFormModel model)
    {
        try
        {
            var responseResult = await GetOrCreateAddressAsync(model);
            if (responseResult.StatusCode == StatusCode.OK)
            {
                var addressEntity = (AddressEntity)responseResult.ContentResult!;
                userEntity.AddressId = addressEntity.Id;

                var updateResult = await _userManager.UpdateAsync(userEntity);

                if (updateResult.Succeeded)
                {
                    return ResponseFactory.Ok("Updated successfully.");
                }
            }
            return ResponseFactory.Error("Something went wrong.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
