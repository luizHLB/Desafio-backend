using Microsoft.Extensions.Logging;
using Moq;
using Product.Domain.DTO.Plan;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Service;

namespace Product.Test
{
    public class PlanServiceTests
    {
        private readonly Mock<ILogger<PlanService>> _loggerMock;
        private readonly Mock<IPlanRepository> _repositoryMock;
        private readonly PlanService _planService;

        public PlanServiceTests()
        {
            _loggerMock = new Mock<ILogger<PlanService>>();
            _repositoryMock = new Mock<IPlanRepository>();
            _planService = new PlanService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task GetPlans_ReturnsListOfPlans()
        {
            // Arrange
            var plans = new List<PlanDTO>
            {
                new PlanDTO(new Plan { Id = 1, Name = "Basic", Price = 9.99 }),
                new PlanDTO(new Plan { Id = 2, Name = "Standard", Price = 19.99 }),
                new PlanDTO(new Plan { Id = 3, Name = "Premium", Price = 29.99 })
            };

            _repositoryMock.Setup(r => r.GetPlans()).ReturnsAsync(plans);

            // Act
            var result = await _planService.GetPlans();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Basic", result[0].Name);
            Assert.Equal("Standard", result[1].Name);
            Assert.Equal("Premium", result[2].Name);
        }

        [Fact]
        public async Task GetPlans_ReturnsEmptyList_WhenNoPlansAvailable()
        {
            // Arrange
            var plans = new List<PlanDTO>();

            _repositoryMock.Setup(r => r.GetPlans()).ReturnsAsync(plans);

            // Act
            var result = await _planService.GetPlans();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}