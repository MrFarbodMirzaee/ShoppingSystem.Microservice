using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Delete;
using ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Update;
using ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAll;
using ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAvailable;
using ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetByCategory;
using ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetById;
using ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Delete;
using ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.Download;
using ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.GetUrl;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Catalog;

public class ProductController 
: BaseController
{
    #region Gets
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableProductsQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory([FromQuery] GetByCategoryProductsQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromQuery] GetByIdProductQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductImageUrl([FromQuery] GetUrlProductImageQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);   
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> DownloadProductImage([FromQuery] DownloadProductImageQuery request, CancellationToken ct) =>
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
    public async Task<IActionResult> Add([FromBody] AddProductCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> AddProductImage([FromForm]AddProductImageCommand request, CancellationToken ct) =>
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
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand request, CancellationToken ct) =>
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
    public async Task<IActionResult> Delete([FromQuery] DeleteProductCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> DeleteProductImage([FromQuery] DeleteProductImageCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    

    #endregion
}