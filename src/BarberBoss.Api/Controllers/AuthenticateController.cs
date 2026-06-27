using BarberBoss.Application.UseCases.Authentication.Authenticate;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Authenticate(
        [FromServices] IAuthenticateUseCase useCase,
        [FromBody] RequestAuthenticateJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}