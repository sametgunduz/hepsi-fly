using MediatR;

namespace HepsiFly.Business.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<string>
{
    public string Id { get; set; }
}