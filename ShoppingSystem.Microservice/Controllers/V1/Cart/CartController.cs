using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.AddByCustomerId;
using ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Delete;
using ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Update;
using ShoppingSystem.Microservice.Application.UseCases.Cart.Queries.GetByCustomerId;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Cart;

[Authorize]
public class CartController 
: BaseController
{
    #region Gets
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomerId([FromQuery] GetByCustomerIdCartQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion

    #region Adds
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddByCustomerId([FromBody] AddCartCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion

    #region Updates
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateCartCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    #endregion

    #region Deletes
    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteById([FromQuery] DeleteCartCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
}