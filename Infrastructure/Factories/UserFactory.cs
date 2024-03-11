using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.Account;
using Infrastructure.Models.Auth;

namespace Infrastructure.Factories;

public class UserFactory
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

            return ResponseFactory.Ok("Populated successfully.");
        }
        catch(Exception ex)
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
            ProfileImage = "images/profile-image.svg",
        };

        return model;
    }
}
