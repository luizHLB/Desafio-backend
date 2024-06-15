using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO;
using Product.Domain.DTO.Rental;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Service;

namespace Product.Test
{
    public class RentalServiceTests
    {
        private readonly Mock<ILogger<RentalService>> _loggerMock;
        private readonly Mock<IRentalRepository> _repositoryMock;
        private readonly Mock<IDriverRepository> _driverRepositoryMock;
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        private readonly RentalService _rentalService;

        public RentalServiceTests()
        {
            _loggerMock = new Mock<ILogger<RentalService>>();
            _repositoryMock = new Mock<IRentalRepository>();
            _driverRepositoryMock = new Mock<IDriverRepository>();
            _planRepositoryMock = new Mock<IPlanRepository>();
            _rentalService = new RentalService(_loggerMock.Object, _repositoryMock.Object, _driverRepositoryMock.Object, _planRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsRentalDTO()
        {
            // Arrange
            var createDto = new CreateRentalDTO { DriverId = 1, PlanId = 1, VehicleId = 1, WithdrawDate = DateTime.Now, EstimatedReturnDate = DateTime.Now.AddDays(7) };
            var rental = new Rental { DriverId = 1, PlanId = 1, VehicleId = 1, WithdrawDate = createDto.WithdrawDate, EstimatedReturnDate = createDto.EstimatedReturnDate };
            var driver = new Driver
            {
                Id = 1,
                Identifier = "DriverIdentifier",
                Name = "DriverName",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = 1,
                CNHImage = $"{Guid.NewGuid}".OnlyAlphaNumeric()
            };

            _driverRepositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(driver);
            // Act
            var result = await _rentalService.Create(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.DriverId, result.DriverId);
            Assert.Equal(createDto.PlanId, result.PlanId);
            Assert.Equal(createDto.VehicleId, result.VehicleId);
            Assert.Equal(createDto.WithdrawDate, result.WithdrawDate);
            Assert.Equal(createDto.EstimatedReturnDate, result.EstimatedReturnDate);
            _repositoryMock.Verify(r => r.Add(It.IsAny<Rental>()), Times.Once);
        }

        [Fact]
        public async Task Complete_ReturnsRentalDTO()
        {
            // Arrange
            var updateDto = new UpdateRentalDTO { Id = 1, ReturnalDate = DateTime.Now };
            var rental = new Rental { Id = 1, PlanId = 1, DriverId = 1, VehicleId = 1, WithdrawDate = DateTime.Now.AddDays(-7), EstimatedReturnDate = DateTime.Now.AddDays(7) };
            var plan = new Plan { Id = 1, Price = 10, Fine = 1, Extra = 2 };
            var driver = new Driver
            {
                Id = 1,
                Identifier = "DriverIdentifier",
                Name = "DriverName",
                CNPJ = "12345678000195",
                BirthDate = DateTime.Now.AddYears(-20),
                CNH = "12345678901",
                CNHCategory = 1,
                CNHImage = $"{Guid.NewGuid}".OnlyAlphaNumeric()
            };


            _driverRepositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(driver);
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(rental);
            _planRepositoryMock.Setup(p => p.GetById(It.IsAny<long>())).ReturnsAsync(plan);

            // Act
            var result = await _rentalService.Complete(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Id, result.Id);
            Assert.Equal(updateDto.ReturnalDate, result.ReturnDate);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Rental>()), Times.Once);
        }

        [Fact]
        public async Task Complete_ThrowsRecordNotFoundException_WhenRentalNotFound()
        {
            // Arrange
            var updateDto = new UpdateRentalDTO { Id = 1, ReturnalDate = DateTime.Now };
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync((Rental)null);

            // Act & Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(() => _rentalService.Complete(updateDto));
        }

        [Fact]
        public async Task GetDtoById_ReturnsRentalDTO()
        {
            // Arrange
            var rental = new Rental { Id = 1, DriverId = 1, PlanId = 1, VehicleId = 1, WithdrawDate = DateTime.Now, EstimatedReturnDate = DateTime.Now.AddDays(7) };
            _repositoryMock.Setup(r => r.GetById(It.IsAny<long>())).ReturnsAsync(rental);

            // Act
            var result = await _rentalService.GetDtoById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rental.Id, result.Id);
        }

        [Fact]
        public async Task PagedListAsync_ReturnsPagedListDTO()
        {
            // Arrange
            var rentals = new List<Rental>
            {
                new Rental { Id = 1, DriverId = 1, PlanId = 1, VehicleId = 1, WithdrawDate = DateTime.Now, EstimatedReturnDate = DateTime.Now.AddDays(7) },
                new Rental { Id = 2, DriverId = 1, PlanId = 1, VehicleId = 1, WithdrawDate = DateTime.Now, EstimatedReturnDate = DateTime.Now.AddDays(7) }
            };
            var pagedList = new PagedListDTO<RentalDTO>(rentals.Select(r => new RentalDTO(r)).ToList(), 2, 10, 1, 1, false, false);

            var validate = false;
            long? driverId = null;
            _repositoryMock.Setup(r => r.PagedListAsync(x => validate ? x.DriverId == driverId.Value : true, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedList);

            // Act
            var result = await _rentalService.PagedListAsync(null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }
    }
}