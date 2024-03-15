using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Account;
using Infrastructure.Models.Auth;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, UserFactory userFactory, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly UserFactory _userFactory = userFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<ResponseResult> RegisterUserAsync(SignUpFormModel model)
    {
        try
        {
            var existResult = await _userRepository.ExistsAsync(x => x.Email == model.Email);

            if (existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var responseResult = _userFactory.PopulateUserEntity(model);

                if(responseResult.StatusCode == StatusCode.OK)
                {
                    var identityResult = await _userManager.CreateAsync((UserEntity)responseResult.ContentResult!, model.Password);

                    if (identityResult.Succeeded)
                        return ResponseFactory.Ok("User created successfully.");

                    return ResponseFactory.Error("Something went wrong with creating the user.");
                }
                else
                    return responseResult;
            }

            return existResult;
        }
        catch (Exception ex) 
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> SignInUserAsync(SignInFormModel model)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);
            if (result.Succeeded)
                return ResponseFactory.Ok("Successfully logged in.");

            return ResponseFactory.Error("Incorrect email or password.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> UpdateBasicInfoAsync(UserEntity userEntity, AccountDetailsBasicFormModel model)
    {
        try
        {
            if (userEntity.Email != model.Email)
            {
                var existResult = await _userRepository.ExistsAsync(x => x.Email == model.Email);
                if (existResult.StatusCode == StatusCode.EXISTS)
                    return ResponseFactory.Exists("A user with that email already exists.");
            }
            
            var responseResult = _userFactory.PopulateUserEntity(model, userEntity);
            var userToUpdate = (UserEntity)responseResult.ContentResult!;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);
            if (updateResult.Succeeded)
                return ResponseFactory.Ok("Updated successfully.");

            return ResponseFactory.Error("Something went wrong.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> RegisterOrUpdateExternalAccountAsync(UserEntity userEntity)
    {
        try
        {
            var existResult = await _userRepository.ExistsAsync(x => x.Email == userEntity.Email);

            if(existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var result = await _userManager.CreateAsync(userEntity);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(userEntity.Email!);
                    return ResponseFactory.Ok(user!, "Created external user successfully.");
                }
            }

            if(existResult.StatusCode == StatusCode.EXISTS)
            {
                var user = await _userManager.FindByEmailAsync(userEntity.Email!);

                if (user!.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email || user.PhoneNumber != userEntity.PhoneNumber || user.Bio != userEntity.Bio)
                {
                    var responseResult = _userFactory.PopulateUserEntity(userEntity, user);
                    await _userManager.UpdateAsync((UserEntity)responseResult.ContentResult!);
                }
                return ResponseFactory.Ok(user, "Found external user.");
            }

            return ResponseFactory.Error("Something went wrong with the registration or update.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
