using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class AccountService(UserRepository userRepository, UserFactory userFactory, AddressFactory addressFactory, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly UserFactory _userFactory = userFactory;
    private readonly AddressFactory _addressFactory = addressFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
}
