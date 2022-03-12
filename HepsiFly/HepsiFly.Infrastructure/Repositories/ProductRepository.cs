using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using HepsiFly.Infrastructure.Base;
using Microsoft.Extensions.Options;

namespace HepsiFly.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }
}