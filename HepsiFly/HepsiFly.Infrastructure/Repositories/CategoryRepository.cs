using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using HepsiFly.Infrastructure.Base;
using Microsoft.Extensions.Options;

namespace HepsiFly.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }
}