using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.DeleteById;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Update;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetAll;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetAvailable;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetById;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetByProductId;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetLowStock;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetOutOfStock;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Inventory;

public class InventoryController 
: BaseController
{
    #region Gets

    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllInventoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableInventoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromQuery] GetByIdInventoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProductId([FromQuery] GetByProductIdInventoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLowStock([FromQuery] GetLowStockInventoryQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOutOfStock([FromQuery] GetOutOfStockInventoryQuery request, CancellationToken ct) =>
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
    public async Task<IActionResult> Add([FromBody] AddInventoryCommand request, CancellationToken ct) =>
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
    public async Task<IActionResult> Update([FromBody] UpdateInventoryCommand request, CancellationToken ct) =>
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
    public async Task<IActionResult> Delete([FromQuery] DeleteByIdInventoryCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
}