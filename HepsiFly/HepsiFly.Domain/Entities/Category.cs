using HepsiFly.Domain.Base;

namespace HepsiFly.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}