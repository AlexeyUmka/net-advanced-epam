using Catalog.Application.Categories.Commands.CreateCategory;
using Catalog.Application.Categories.Commands.DeleteCategory;
using Catalog.Application.Categories.Commands.UpdateCategory;
using Catalog.Application.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers;

public class CategoriesController : ApiControllerBase
{
    [HttpGet("{id:int?}")]
    public async Task<ActionResult<CategoriesVm>> GetCategories(int? id = null)
    {
        var query = new GetCategoriesQuery() { Id = id };
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand() { Id = id });

        return NoContent();
    }
}
