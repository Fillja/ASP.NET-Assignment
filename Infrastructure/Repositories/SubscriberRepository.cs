using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class SubscriberRepository(DataContext context) : BaseRepository<SubscriberEntity>(context)
{
    private readonly DataContext _context = context;
}
