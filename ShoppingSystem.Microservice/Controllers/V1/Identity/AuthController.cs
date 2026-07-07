using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystem.Microservice.Application.UseCases.Identity.GoogleSignIn.Commands;
using ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command;
using ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command.Revoke;
using ShoppingSystem.Microservice.Application.UseCases.Identity.SignIn.Commands;
using ShoppingSystem.Microservice.Application.UseCases.Identity.SignUp.Commands;
using ShoppingSystem.Microservice.Controllers.Base;

namespace ShoppingSystem.Microservice.Controllers.V1.Identity;

public class AuthController 
: BaseController
{
    #region Gets

    

    #endregion
    
    #region Adds
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenCommandRequest request, CancellationToken ct) =>
        await SendAsync(request, ct);
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestCommand request, CancellationToken ct) =>
        await SendAsync(request, ct);

    #endregion
    
    #region Updates

    

    #endregion
    
    #region Deletes

    

    #endregion
}