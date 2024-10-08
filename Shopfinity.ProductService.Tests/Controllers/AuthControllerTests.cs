using Dynamitey;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shopfinity.ProductService.Interfaces;
using Shopfinity.ProductService.Models;

public class AuthControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        // Mock UserManager
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

        // Mock TokenService
        _tokenServiceMock = new Mock<ITokenService>();

        // Mock Logger
        _loggerMock = new Mock<ILogger<AuthController>>();

        // Instantiate AuthController
        _authController = new AuthController(_userManagerMock.Object, _tokenServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GenerateToken_ValidCredentials_ReturnsOkResultWithToken()
    {
        // Arrange
        var loginModel = new LoginModel { Username = "testuser", Password = "password123" };
        var user = new ApplicationUser { Id = "1", UserName = loginModel.Username };
        var token = "valid-jwt-token";

        _userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(ts => ts.GenerateTokenAsync(user)).ReturnsAsync(token);

        // Act
        var result = await _authController.GenerateToken(loginModel) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(token, Dynamic.InvokeGet(result.Value, "Token"));
    }

    [Fact]
    public async Task GenerateToken_InvalidUsername_ReturnsUnauthorized()
    {
        // Arrange
        var loginModel = new LoginModel { Username = "unknownuser", Password = "password123" };

        _userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username)).ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _authController.GenerateToken(loginModel) as UnauthorizedObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
        Assert.Equal("Invalid username or password.", result.Value);
    }

    [Fact]
    public async Task GenerateToken_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var loginModel = new LoginModel { Username = "testuser", Password = "wrongpassword" };
        var user = new ApplicationUser { Id = "1", UserName = loginModel.Username };

        _userManagerMock.Setup(um => um.FindByNameAsync(loginModel.Username)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(false);

        // Act
        var result = await _authController.GenerateToken(loginModel) as UnauthorizedObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
        Assert.Equal("Invalid username or password.", result.Value);
    }
}