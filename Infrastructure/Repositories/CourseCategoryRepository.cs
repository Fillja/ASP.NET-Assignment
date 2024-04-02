using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CourseCategoryRepository(DataContext context) : BaseRepository<CourseCategoryEntity>(context)
{
    private readonly DataContext _context = context;
}