using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Shopfinity.ProductService.Interfaces;
using Shopfinity.ProductService.Models;
using Shopfinity.ProductService.Services;
using System.Security.Claims;

namespace Shopfinity.ProductService.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
        private readonly ITokenService _tokenService;

        public TokenServiceTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            // Set up mock JWT settings
            _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
            _jwtSettingsMock.Setup(x => x.Value).Returns(new JwtSettings
            {
                Key = "your-256-bit-secret-your-256-bit-secret",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpiryMinutes = 60
            });

            _tokenService = new TokenService(_jwtSettingsMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task GenerateTokenAsync_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1", UserName = "testuser" };
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };
            var roles = new List<string> { "Admin", "User" };

            _userManagerMock.Setup(um => um.GetClaimsAsync(user)).ReturnsAsync(claims);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roles);

            // Act
            var token = await _tokenService.GenerateTokenAsync(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task GenerateTokenAsync_UserHasNoClaimsOrRoles_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { Id = "2", UserName = "userWithoutClaims" };

            _userManagerMock.Setup(um => um.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            // Act
            var token = await _tokenService.GenerateTokenAsync(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task GenerateTokenAsync_InvalidUser_ThrowsException()
        {
            // Arrange
            ApplicationUser nullUser = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.GenerateTokenAsync(nullUser));
        }
    }
}