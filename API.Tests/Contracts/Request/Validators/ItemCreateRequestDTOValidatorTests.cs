using API.Contracts.Request;
using API.Contracts.Request.Validators;
using API.Data.Interfaces;
using FluentValidation.Results;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace API.Tests.Contracts.Request.Validators
{
    public class ItemCreateRequestDTOValidatorTests
    {
        [Fact]
        public void ItemCreateRequestDTO_HasValidName()
        {
            // Arrange
            Mock<IItemRepository> mockRepo = new();
            ItemCreateRequestValidator validator = new(mockRepo.Object);
            const string nullName = null;
            const string blankName = "";
            const string whitespaceName = "   ";
            const string invalidCharactersName = "Test-name_1";
            const string validName = "Test name 1";
            ItemCreateRequestDTO nullNameIsInvalid = new() { Name = nullName };
            ItemCreateRequestDTO blankNameIsInvalid = new() { Name = blankName };
            ItemCreateRequestDTO whitespaceNameIsInvalid = new() { Name = whitespaceName };
            ItemCreateRequestDTO invalidCharactersNameIsInvalid = new() { Name = invalidCharactersName };
            ItemCreateRequestDTO validNameIsValid = new() { Name = validName };


            // Act
            ValidationResult nullNameValidatorResult = validator.Validate(nullNameIsInvalid);
            ValidationResult blankNameValidatorResult = validator.Validate(blankNameIsInvalid);
            ValidationResult whitespaceNameValidatorResult = validator.Validate(whitespaceNameIsInvalid);
            ValidationResult invalidCharactersNameValidatorResult = validator.Validate(invalidCharactersNameIsInvalid);
            ValidationResult validNameValidatorResult = validator.Validate(validNameIsValid);


            // Assert
            Assert.True(nullNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(nullNameIsInvalid.Name)).Count() > 0
                , $"{nameof(nullNameIsInvalid.Name)} value of null should not be valid.");

            Assert.True(blankNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(blankNameIsInvalid.Name)).Count() > 0
                , $"{nameof(blankNameIsInvalid.Name)} value '{blankName}' should not be valid.");

            Assert.True(whitespaceNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(whitespaceNameIsInvalid.Name)).Count() > 0
                , $"{nameof(whitespaceNameIsInvalid.Name)} value '{whitespaceName}' should not be valid.");

            Assert.True(invalidCharactersNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(invalidCharactersNameIsInvalid.Name)).Count() > 0
                , $"{nameof(invalidCharactersNameIsInvalid.Name)} value '{invalidCharactersName}' should not be valid.");

            Assert.True(validNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(validNameIsValid.Name)).Count() == 0
                , $"{nameof(validNameIsValid.Name)} value '{validName}' should be valid.");

        }

        [Fact]
        public void ItemCreateRequestDTO_ChecksForUniqueName()
        {
            // Arrange
            Mock<IItemRepository> mockRepo = new();
            ItemCreateRequestValidator validator = new(mockRepo.Object);
            ItemCreateRequestDTO item = new() { Name = "Test name" };

            // Act
            ValidationResult validatorResult = validator.Validate(item);


            // Assert
            mockRepo.Verify(repo => repo.NameExistsAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ItemCreateRequestDTO_HasValidActiveStartDate()
        {
            
            // Arrange
            Mock<IItemRepository> mockRepo = new();
            ItemCreateRequestValidator validator = new(mockRepo.Object);
            DateTimeOffset defaultDateTimeOffset = default(DateTimeOffset);
            Data.Models.Item item2 = new();
            ItemCreateRequestDTO item = new() { ActiveStartDate = defaultDateTimeOffset };

            // Act
            ValidationResult validatorResult = validator.Validate(item);


            // Assert
            Assert.True(validatorResult.Errors
                .Where(e => e.PropertyName == nameof(item.ActiveStartDate)).Count() > 0
                , $"{nameof(item.ActiveStartDate)} value '{defaultDateTimeOffset}' should not be valid.");
        }
    }
}
