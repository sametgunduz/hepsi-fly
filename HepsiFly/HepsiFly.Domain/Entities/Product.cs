using HepsiFly.Domain.Base;
using HepsiFly.Domain.Enums;

namespace HepsiFly.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
}