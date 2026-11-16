using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
[Produces("application/json")]
public class AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase) : ControllerBase
{
    
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await registerUseCase.ExecuteAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request) =>
        Ok(await loginUseCase.ExecuteAsync(request));
}
