using Microsoft.Extensions.Configuration;
using Moq;
using Product.Domain.DTO.Authentication;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Settings;
using Product.Service;

namespace Product.Test
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _configMock = new Mock<IConfiguration>();
            _repositoryMock = new Mock<IUserRepository>();

            // Configurando mocks
            var jwtSettings = new JwtSettings
            {
                Key = "ACDt1vR3lXToPQ1g3MyN",
                SecKey = "163,236,107,188,102,57,85,81,221,44,144,71,40,139,185,110,26,114,35,131,140,183,233,167,203,126,30,111,51,124,114,80",
                IV = "228,115,168,37,249,9,11,153,136,171,40,167,217,127,151,22",
                LifeTime = 1440,
                Issuer = "https://localhost:7045/",
                Audience = "https://localhost:7045/"
            };

            var inMemorySettings = new Dictionary<string, string>
            {
                {nameof(JwtSettings), Newtonsoft.Json.JsonConvert.SerializeObject(jwtSettings)}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _configMock.Setup(c => c.GetSection(nameof(JwtSettings))).Returns(configuration.GetSection(nameof(JwtSettings)));

            _authService = new AuthenticationService(_configMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var dto = new AuthenticationDTO
            {
                Data = "ywUB54Vih5gwAfVhHbEwVqhAI85hRzeYyeo1yLyWIuk="
            };

            var user = new User
            {
                Id = 1,
                Email = "testUser@product.com",
                Name = "User Test"
            };

            _repositoryMock.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(user);

            // Act
            var result = await _authService.Authenticate(dto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ThrowsLoginException()
        {
            // Arrange
            var dto = new AuthenticationDTO
            {
                Data = "ywUB54Vih5gwAfVhHbEwVqhAI85hRzeYyeo1yLyWIuk="
            };

            _repositoryMock.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<LoginException>(() => _authService.Authenticate(dto));
        }
    }

}
