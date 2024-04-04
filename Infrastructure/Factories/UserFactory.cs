using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.Account;
using Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Factories;

public class UserFactory()
{

    public ResponseResult PopulateUserEntity(SignUpFormModel model)
    {
        try
        {
            var result = new UserEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            };
            return ResponseFactory.Ok(result, "Populated successfully.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public ResponseResult PopulateUserEntity(AccountDetailsBasicFormModel model, UserEntity entity)
    {
        try
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.UserName = model.Email;
            entity.PhoneNumber = model.Phone;
            entity.Bio = model.Biography;

            return ResponseFactory.Ok(entity,"Populated successfully.");
        }
        catch(Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public ResponseResult PopulateUserEntity(ExternalLoginInfo info)
    {
        try
        {
            var result = new UserEntity
            {
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                IsExternalAccount = true,
            };
            return ResponseFactory.Ok(result, "Populated successfully.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public ResponseResult PopulateUserEntity(UserEntity userEntity, UserEntity identityUser)
    {
        try
        {
            identityUser.FirstName = userEntity.FirstName;
            identityUser.LastName = userEntity.LastName;
            identityUser.Email = userEntity.Email;
            identityUser.PhoneNumber = userEntity.PhoneNumber;
            identityUser.Email = userEntity.Email;

            return ResponseFactory.Ok(identityUser, "Populated successfully.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public AccountDetailsBasicFormModel PopulateBasicForm(UserEntity userEntity)
    {
        var model = new AccountDetailsBasicFormModel
        {
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            Email = userEntity.Email!,
            Phone = userEntity.PhoneNumber!,
            Biography = userEntity.Bio,
            ProfileImage = userEntity.ImageUrl,
            IsExternalAccount = userEntity.IsExternalAccount,
        };

        return model;
    }
}
