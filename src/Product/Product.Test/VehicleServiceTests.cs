using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service;

namespace Product.Test
{
    public class VehicleServiceTests
    {
        private readonly Mock<ILogger<VehicleService>> _loggerMock;
        private readonly Mock<IVehicleRepository> _repositoryMock;
        private readonly Mock<IRabbitMQManager> _rabbitMQManagerMock;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _loggerMock = new Mock<ILogger<VehicleService>>();
            _repositoryMock = new Mock<IVehicleRepository>();
            _rabbitMQManagerMock = new Mock<IRabbitMQManager>();
            _vehicleService = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _rabbitMQManagerMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsVehicleDTO()
        {
            // Arrange
            var createDto = new CreateVehicleDTO { Identifier = "12345", LicensePlate = "ABC1234", Model = "ModelX", Year = 2022 };
            var vehicle = new Vehicle { Identifier = createDto.Identifier, LicensePlate = createDto.LicensePlate, Model = createDto.Model, Year = createDto.Year };

            // Act
            var result = await _vehicleService.Create(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Identifier, result.Identifier);
            Assert.Equal(createDto.LicensePlate, result.LicensePlate);
            Assert.Equal(createDto.Model, result.Model);
            Assert.Equal(createDto.Year, result.Year);
            _repositoryMock.Verify(r => r.Add(It.IsAny<Vehicle>()), Times.Once);
            _rabbitMQManagerMock.Verify(r => r.BasicPublish(It.IsAny<ExecutionQueue>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetDtoById_ReturnsVehicleDTO()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1, Identifier = "12345", LicensePlate = "ABC1234", Model = "ModelX", Year = 2022 };
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(vehicle);

            // Act
            var result = await _vehicleService.GetDtoById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vehicle.Id, result.Id);
        }

        [Fact]
        public async Task GetDtoById_ThrowsRecordNotFoundException_WhenVehicleNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync((Vehicle)null);

            // Act & Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(() => _vehicleService.GetDtoById(1));
        }

        [Fact]
        public async Task PagedListAsync_ReturnsPagedListDTO()
        {
            // Arrange
            var vehicles = new List<Vehicle>
        {
            new Vehicle { Id = 1, Identifier = "12345", LicensePlate = "ABC1234", Model = "ModelX", Year = 2022 },
            new Vehicle { Id = 2, Identifier = "12346", LicensePlate = "XYZ5678", Model = "ModelY", Year = 2023 }
        };
            var pagedList = new PagedListDTO<VehicleDTO>(vehicles.Select(v => new VehicleDTO(v)).ToList(), vehicles.Count, 1, 1, 10, false, false);

            _repositoryMock.Setup(r => r.PagedListAsync(w => w.LicensePlate.ToLower().Contains("abc"), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedList);

            // Act
            var result = await _vehicleService.PagedListAsync("ABC", 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task Update_ReturnsVehicleDTO()
        {
            // Arrange
            var patchDto = new PatchVehicleDTO { Id = 1, LicensePlate = "XYZ5678" };
            var vehicle = new Vehicle { Id = patchDto.Id, Identifier = "12345", LicensePlate = "ABC1234", Model = "ModelX", Year = 2022 };

            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(vehicle);

            // Act
            var result = await _vehicleService.Update(patchDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patchDto.LicensePlate, result.LicensePlate);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Vehicle>()), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsRecordNotFoundException_WhenVehicleNotFound()
        {
            // Arrange
            var patchDto = new PatchVehicleDTO { Id = 1, LicensePlate = "XYZ5678" };
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync((Vehicle)null);

            // Act & Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(() => _vehicleService.Update(patchDto));
        }
    }
}
