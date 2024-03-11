using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Auth;
using Infrastructure.Repositories;
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

    public async Task<ResponseResult> UpdateUser(UserEntity userEntity)
    {
        return null!;
    }
}
