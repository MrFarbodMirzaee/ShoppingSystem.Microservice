using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.DeleteById;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Update;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetAll;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCity;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCountry;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetById;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByPostalCode;
using ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByState;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Address;

public class AddressController 
: BaseController
{
    #region Gets
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery]GetAllAddressQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromQuery] GetByIdAddressQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCity([FromQuery] GetByCityAddressQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCountry([FromQuery] GetByCountryAddressQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPostalCode([FromQuery] GetByPostalCodeAddressQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByState([FromQuery] GetByStateAddressQuery request, CancellationToken ct) =>
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
    public async Task<IActionResult> Add([FromBody] AddAddressRequestCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
    
    #region Updates
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateAddressCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);
    #endregion
    
    #region Deletes

    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] DeleteByIdAddressCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
}