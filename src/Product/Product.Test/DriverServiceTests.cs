using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;
using Product.Domain.Entities.Enums;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service;
using System.Text;

namespace Product.Test
{
    public class DriverServiceTests
    {
        private readonly Mock<ILogger<DriverService>> _loggerMock;
        private readonly Mock<IDriverRepository> _repositoryMock;
        private readonly Mock<IAzureStorage> _azureStorageMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly DriverService _driverService;

        public DriverServiceTests()
        {
            _loggerMock = new Mock<ILogger<DriverService>>();
            _repositoryMock = new Mock<IDriverRepository>();
            _azureStorageMock = new Mock<IAzureStorage>();
            _configMock = new Mock<IConfiguration>();

            var azureSettings = new AzureStorageSettings
            {
                BaseURI = "http://someurl",
                ContainerName = "container"
            };

            var inMemorySettings = new Dictionary<string, string>
            {
                {nameof(AzureStorageSettings), Newtonsoft.Json.JsonConvert.SerializeObject(azureSettings)}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _configMock.Setup(c => c.GetSection(nameof(AzureStorageSettings))).Returns(configuration.GetSection(nameof(AzureStorageSettings)));

            _driverService = new DriverService(_loggerMock.Object, _repositoryMock.Object, _azureStorageMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task Create_ValidDriver_ReturnsDriverDTO()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "DriverName",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            // Act
            var result = await _driverService.Create(createDriverDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DriverName", result.Name);
            Assert.Equal("http://someurl/container/uploadedFileName", result.CNHImage);
        }

        [Fact]
        public async Task GetDtoById_ValidId_ReturnsDriverDTO()
        {
            // Arrange
            var driver = new Driver
            {
                Id = 1,
                Identifier = "DriverIdentifier",
                Name = "DriverName",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = EnumHelper<CNHCategory>.GetValue(new CNHCategory[] { CNHCategory.A }),
                CNHImage = "uploadedFileName"
            };

            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>()))
                           .ReturnsAsync(driver);

            // Act
            var result = await _driverService.GetDtoById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DriverName", result.Name);
            Assert.Equal("http://someurl/container/uploadedFileName", result.CNHImage);
        }

        [Fact]
        public async Task Update_ValidDriver_ReturnsUpdatedDriverDTO()
        {
            // Arrange
            var updateDriverDto = new UpdateDriverDTO
            {
                CNHImage = CreateFormFile("cnh_updated.png")
            };

            var driver = new Driver
            {
                Id = 1,
                Identifier = "DriverIdentifier",
                Name = "DriverName",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = EnumHelper<CNHCategory>.GetValue(new CNHCategory[] { CNHCategory.A }),
                CNHImage = "uploadedFileName"
            };

            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>()))
                           .ReturnsAsync(driver);

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("updatedUploadedFileName");

            // Act
            var result = await _driverService.Update(1, updateDriverDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("http://someurl/container/updatedUploadedFileName", result.CNHImage);
        }

        [Fact]
        public async Task Remove_ValidId_RemovesDriver()
        {
            // Arrange
            var driver = new Driver
            {
                Id = 1,
                CNHImage = "uploadedFileName"
            };

            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>()))
                           .ReturnsAsync(driver);

            // Act
            await _driverService.Remove(1);

            // Assert
            _azureStorageMock.Verify(a => a.DeleteFile("uploadedFileName"), Times.Once);
            _repositoryMock.Verify(r => r.Remove(driver), Times.Once);
        }

        private IFormFile CreateFormFile(string fileName)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("file content"));
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            return fileMock.Object;
        }
    }

}
