using API.Contracts.Response;
using API.Controllers;
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests.Controllers
{
    public class ItemController_MoveTests
    {
        [Fact]
        public async Task Move_ReturnsNotFound_GivenInvalidGeocacheID()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo;
            Mock<IItemRepository> mockItemRepo;
            _setupMockRepos(out mockGeocacheRepo, out mockItemRepo);
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);

            // Act
            var result = await controller.MoveItem(targetGeocacheID: _testNotFoundGeocacheID, itemID: _testItemID);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Move_ReturnsNotFound_GivenInvalidItemID()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo;
            Mock<IItemRepository> mockItemRepo;
            _setupMockRepos(out mockGeocacheRepo, out mockItemRepo);
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);

            // Act
            var result = await controller.MoveItem(targetGeocacheID: _testGeocacheID, itemID: _testNotFoundItemID);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Move_ReturnsUnprocessableEntity_GivenInactiveItemID()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo;
            Mock<IItemRepository> mockItemRepo;
            _setupMockRepos(out mockGeocacheRepo, out mockItemRepo);
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);

            // Act
            var result = await controller.MoveItem(targetGeocacheID: _testGeocacheID, itemID: _testInactiveItemID);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [Fact]
        public async Task Move_ReturnsUnprocessableEntity_GivenTargetGeocacheIDWithTooManyItems()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo;
            Mock<IItemRepository> mockItemRepo;
            _setupMockRepos(out mockGeocacheRepo, out mockItemRepo);
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);

            // Act
            var result = await controller.MoveItem(targetGeocacheID: _testExcessItemsGeocacheID, itemID: _testItemID);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [Fact]
        public async Task Move_ReturnsNoContent_OnSuccessfulMove()
        {
            // Arrange
            Mock<IGeocacheRepository> mockGeocacheRepo;
            Mock<IItemRepository> mockItemRepo;
            _setupMockRepos(out mockGeocacheRepo, out mockItemRepo);
            ItemController controller = new(mockGeocacheRepo.Object, mockItemRepo.Object);

            // Act
            var result = await controller.MoveItem(targetGeocacheID: _testGeocacheID, itemID: _testItemID);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
        }

        private readonly int _testNotFoundGeocacheID = 0;
        private readonly int _testGeocacheID = 1;
        private readonly int _testExcessItemsGeocacheID = 2;

        private readonly int _testNotFoundItemID = 0;
        private readonly int _testItemID = 1;
        private readonly int _testInactiveItemID = 2;
        private void _setupMockRepos(out Mock<IGeocacheRepository> outGeocacheRepo, out Mock<IItemRepository> outItemRepo)
        {
            outGeocacheRepo = new();
            outItemRepo = new();

            outGeocacheRepo.Setup(repo => repo.FindAsync(_testGeocacheID))
                .Returns(Task.FromResult(
                    new Geocache()
                    {
                        ID = _testGeocacheID,
                        CacheItems = new List<Item>()
                    }));

            outGeocacheRepo.Setup(repo => repo.FindAsync(_testExcessItemsGeocacheID))
                .Returns(Task.FromResult(
                    new Geocache()
                    {
                        ID = _testExcessItemsGeocacheID,
                        CacheItems = new List<Item>() { new(), new(), new(), new() }
                    }));

            outItemRepo.Setup(repo => repo.FindAsync(_testItemID))
                .Returns(Task.FromResult(
                    new Item()
                    {
                        ID = _testItemID
                    }));

            outItemRepo.Setup(repo => repo.FindAsync(_testInactiveItemID))
                .Returns(Task.FromResult(
                    new Item()
                    {
                        ID = _testInactiveItemID,
                        ActiveStartDate = new DateTimeOffset(new DateTime(year: 2020, month: 12, day: 1)),
                        ActiveEndDate = new DateTimeOffset(new DateTime(year: 2020, month: 12, day: 2))
                    }));
        }
    }
}
