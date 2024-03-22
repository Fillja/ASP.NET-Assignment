using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CourseRepository(DataContext context) : BaseRepository<CourseEntity>(context)
{
    private readonly DataContext _dataContext = context;
}
