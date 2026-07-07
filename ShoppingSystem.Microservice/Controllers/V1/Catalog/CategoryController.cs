using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Category.Commands.DeleteById;
using ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Update;
using ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetAll;
using ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByActive;
using ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetById;
using ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByName;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Catalog;

public class CategoryController 
: BaseController
{
    #region Gets
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByActive([FromQuery] GetByActiveCategoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromQuery] GetByIdCategoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByName([FromQuery] GetByNameCategoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    #endregion

    #region Adds

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] AddCategoryCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion

    #region Updates

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion

    #region Deletes

    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] DeleteByIdCategoryCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
}