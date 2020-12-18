using API.Contracts.Response;
using API.Controllers;
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests.Controllers
{
    public class ItemController_CreateTests
    {
        [Fact]
        public async Task Create_ReturnsNotFound_GivenInvalidGeocacheID()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo = getMockGeocacheRepo();
            Mock<IItemRepository> mockItemRepo = new();
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);
            const string testName = "Test Item 1";

            // Act
            var result = await controller.Create(geocacheID: 0, model: new()
            {
                Name = testName
            });

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo = getMockGeocacheRepo();
            Mock<IItemRepository> mockItemRepo = new();
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);
            controller.ModelState.AddModelError("MockErrorMessage", "Mock error message.");

            // Act
            var result = await controller.Create(geocacheID: this._testGeocacheID, model: new());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedItem()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo = getMockGeocacheRepo();
            Mock<IItemRepository> mockItemRepo = new();
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);
            const string testName = "Test Item 1";

            // Act
            var result = await controller.Create(geocacheID: this._testGeocacheID, model: new()
            {
                Name = testName
            });

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ItemResponseDTO>(actionResult.Value);

            mockItemRepo.Verify(repo => repo.CreateAsync(It.IsAny<Item>()), Times.Once);

            Assert.Equal(testName, returnValue.Name);
        }

        private readonly int _testGeocacheID = 4200;
        private Mock<IGeocacheRepository> getMockGeocacheRepo()
        {
            Mock<IGeocacheRepository> result = new();

            result.Setup(repo => repo.FindAsync(_testGeocacheID))
                .Returns(Task.FromResult(
                    new Geocache() { ID = _testGeocacheID, 
                        Name = "Test Geocache 1",
                        CacheItems = new List<Item>() 
                }));

            return result;
        }
    }
}
