using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AddressService(AddressRepository addressRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;
}
