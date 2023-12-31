﻿using Catalog.Application.Common.Models;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers;

public class ProductsController : ApiControllerBase
{
    [HttpGet("{id:int?}")]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Read)]
    public async Task<ActionResult<ProductsVm>> GetProducts(int? id = null)
    {
        var query = new GetProductsQuery() { Id = id };
        return await Mediator.Send(query);
    }
    
    [HttpGet]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Read)]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetProductsByCategoryIdPaginated([FromQuery]int categoryId, [FromQuery]int pageNumber, [FromQuery]int pageSize)
    {
        var query = new GetProductsByCategoryIdQuery() { CategoryId = categoryId, PageNumber = pageNumber, PageSize = pageSize};
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Create)]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Constants.AuthorizationConstants.Policies.Update)]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.ProductToUpdate.Id)
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
        await Mediator.Send(new DeleteProductCommand() { Id = id });

        return NoContent();
    }
}
