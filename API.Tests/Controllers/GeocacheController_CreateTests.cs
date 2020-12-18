using API.Contracts.Response;
using API.Controllers;
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests.Controllers
{
    public class GeocacheController_CreateTests
    {
        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            Mock<IGeocacheRepository> mockRepo = new();
            GeocacheController controller = new(mockRepo.Object);
            controller.ModelState.AddModelError("MockErrorMessage", "Mock error message.");

            // Act
            var result = await controller.Create(model: new());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedGeocache()
        {
            // Arrange
            Mock<IGeocacheRepository> mockRepo = new();
            GeocacheController controller = new(mockRepo.Object);
            const string testName = "Test Geocache 1";
            const decimal testLatitude = 10;
            const decimal testLongitude = 20;

            // Act
            var result = await controller.Create(model: new()
            {
                Name = testName,
                Latitude = testLatitude,
                Longitude = testLongitude
            });

            // Assert

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<GeocacheResponseDTO>(actionResult.Value);

            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Geocache>()), Times.Once);

            Assert.Equal(testName, returnValue.Name);
            Assert.Equal(testLatitude, returnValue.Latitude);
            Assert.Equal(testLongitude, returnValue.Longitude);
        }
    }
}
