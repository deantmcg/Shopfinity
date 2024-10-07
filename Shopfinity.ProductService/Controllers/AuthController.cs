using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopfinity.ProductService.Interfaces;
using Shopfinity.ProductService.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken([FromBody] LoginModel model)
    {
        // Find user by username
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            // Use the TokenService to generate a token
            var token = await _tokenService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid username or password.");
    }
}