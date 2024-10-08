using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopfinity.ProductService.Interfaces;
using Shopfinity.ProductService.Models;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Generates a JWT token for valid users.
    /// </summary>
    /// <param name="model">Login credentials for the user.</param>
    /// <returns>JWT token if authentication is successful, otherwise unauthorized response.</returns>
    [HttpPost("token")]
    [SwaggerOperation(Summary = "Generate JWT Token", Description = "Authenticates the user and generates a JWT token if the credentials are valid.")]
    public async Task<IActionResult> GenerateToken([FromBody] LoginModel model)
    {
        _logger.LogInformation("Generating token for user {Username}", model.Username);
        // Find user by username
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            // Use the TokenService to generate a token
            var token = await _tokenService.GenerateTokenAsync(user);
            _logger.LogInformation("Token generated successfully for user {Username}", model.Username);
            return Ok(new { Token = token });
        }

        _logger.LogWarning("Unauthorized login attempt for user {Username}", model.Username);
        return Unauthorized("Invalid username or password.");
    }
}