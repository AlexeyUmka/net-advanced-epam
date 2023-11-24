using Catalog.Application.Categories.Commands.CreateCategory;
using Catalog.Application.Categories.Commands.DeleteCategory;
using Catalog.Application.Categories.Commands.UpdateCategory;
using Catalog.Application.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers;

public class CategoriesController : ApiControllerBase
{
    [HttpGet("{id:int?}")]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Read)]
    public async Task<ActionResult<CategoriesVm>> GetCategories(int? id = null)
    {
        var query = new GetCategoriesQuery() { Id = id };
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Create)]
    public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Update)]
    public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
    {
        if (id != command.CategoryToUpdate.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Delete)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand() { Id = id });

        return NoContent();
    }
}
