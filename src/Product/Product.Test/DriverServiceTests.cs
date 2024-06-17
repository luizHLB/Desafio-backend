using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;
using Product.Domain.Entities.Enums;
using Product.Domain.Exceptions;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

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
        public async Task Create_ValidDriver()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
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
            Assert.Equal("http://someurl/container/uploadedFileName", result.CNHImage);
        }

        [Fact]
        public async Task Create_InvalidDriver_InvalidImageFormat()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.jpg")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            await Assert.ThrowsAsync<EntityConstraintException>(() => _driverService.Create(createDriverDto));
        }

        [Fact]
        public async Task Create_InvalidDriver_NoImage()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.C, CNHCategory.D, CNHCategory.E },
                //CNHImage = CreateFormFile("cnh.png")
            };

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("CNH Image is required"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_NoName()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("Name is required"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_NoCNPJ()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                //CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("CNPJ is required"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_NoBirthDate()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                //BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("Birth Date invalid"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_InvalidBirthDate()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-17),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("Birth Date invalid"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_NoCNH()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                //CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("CNH is required"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_NoCNHCategory()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                //CNHCategory = new CNHCategory[] { CNHCategory.A },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("CNH Category must be"));
            }
        }

        [Fact]
        public async Task Create_InvalidDriver_InvalidCNHCategory()
        {
            // Arrange
            var createDriverDto = new CreateDriverDTO
            {
                Identifier = "DriverIdentifier",
                Name = "Driver Name",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = new CNHCategory[] { CNHCategory.C, CNHCategory.D, CNHCategory.E },
                CNHImage = CreateFormFile("cnh.png")
            };

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("uploadedFileName");

            var task = _driverService.Create(createDriverDto);
            await Assert.ThrowsAsync<EntityConstraintException>(() => task);
            try
            {
                await task;
                Assert.Fail("Exception should have happened");
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("CNH Category must be"));
            }
        }

        [Fact]
        public async Task GetDtoById_InvalidId()
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

            _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(driver);

            // Act
            await Assert.ThrowsAsync<RecordNotFoundException>(() => _driverService.GetDtoById(2));
        }

        [Fact]
        public async Task Update_ValidDriver()
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
        public async Task Update_InvalidDriver_WrongId()
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

            _repositoryMock.Setup(r => r.GetById(1))
                           .ReturnsAsync(driver);

            _azureStorageMock.Setup(a => a.UploadFile(It.IsAny<IFormFile>()))
                             .ReturnsAsync("updatedUploadedFileName");

            await Assert.ThrowsAsync<RecordNotFoundException>(() => _driverService.Update(2, updateDriverDto));
        }

        [Fact]
        public async Task Update_InvalidDriver_InvalidImageFormat()
        {
            // Arrange
            var updateDriverDto = new UpdateDriverDTO
            {
                CNHImage = CreateFormFile("cnh_updated.jpg")
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

            await Assert.ThrowsAsync<EntityConstraintException>(() => _driverService.Update(1, updateDriverDto));
        }

        [Fact]
        public async Task Update_InvalidDriver_NoImage()
        {
            // Arrange
            var updateDriverDto = new UpdateDriverDTO();

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

            await Assert.ThrowsAsync<EntityConstraintException>(() => _driverService.Update(1, updateDriverDto));
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

        [Fact]
        public async Task Remove_InvalidId()
        {
            // Arrange
            var driver = new Driver
            {
                Id = 1
            };

            _repositoryMock.Setup(r => r.GetById(1))
                           .ReturnsAsync(driver);

            await Assert.ThrowsAsync<RecordNotFoundException>(() => _driverService.Remove(2));
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
