using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Fail;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Refund;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetAll;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetById;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByOrderId;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByStatus;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByTransactionId;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetFailed;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetRefunded;
using ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetSuccessful;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Payment;

[Authorize]
public class PaymentController 
: BaseController
{
    #region Gets

    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromQuery] GetByIdPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByOrderId([FromQuery] GetByOrderIdPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);

    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus([FromQuery] GetByStatusPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTransactionId([FromQuery] GetByTransactionIdPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFailed([FromQuery] GetFailPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRefunded([FromQuery] GetRefundedPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSuccessful([FromQuery] GetSuccessfulPaymentQuery request, CancellationToken ct) =>
        await SendAsync(request, ct);
    #endregion

    #region Adds

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Add([FromBody] AddPaymentCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MakeFail([FromBody] FailPaymentCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MakeRefund([FromBody] RefundPaymentCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion

    #region Updates

    

    #endregion

    #region Deletes

    

    #endregion
}