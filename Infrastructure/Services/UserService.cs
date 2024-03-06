using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository)
{
    private readonly UserRepository _userRepository = userRepository;
}
