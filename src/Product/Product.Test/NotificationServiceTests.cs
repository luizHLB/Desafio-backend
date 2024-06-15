using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO;
using Product.Domain.DTO.Notification;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Service;

namespace Product.Test
{
    public class NotificationServiceTests
    {
        private readonly Mock<ILogger<NotificationService>> _loggerMock;
        private readonly Mock<INotificationRepository> _repositoryMock;
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _repositoryMock = new Mock<INotificationRepository>();
            _notificationService = new NotificationService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task PagedListAsync_ReturnsPagedList()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, Message = "Message 1", Read = false },
                new Notification { Id = 2, Message = "Message 2", Read = false }
            };

            var pagedList = new PagedListDTO<NotificationDTO>(notifications.Select(n => new NotificationDTO(n)).ToList(), notifications.Count, 1, 1, 10, false, false);

            _repositoryMock.Setup(r => r.PagedListAsync(x => !x.Read, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedList);

            // Act
            var result = await _notificationService.PagedListAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            _repositoryMock.Verify(r => r.SetRead(It.Is<IEnumerable<long>>(ids => ids.SequenceEqual(notifications.Select(n => n.Id)))), Times.Once);
        }

        [Fact]
        public async Task Register_ValidVehicle_AddsNotification()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Year = 2020,
                LicensePlate = "ABC1234"
            };

            // Act
            await _notificationService.Register(vehicle);

            // Assert
            _repositoryMock.Verify(r => r.Add(It.Is<Notification>(n => n.Message == "New Vehicle Year: 2020, License Plage: ABC1234")), Times.Once);
        }
    }
}