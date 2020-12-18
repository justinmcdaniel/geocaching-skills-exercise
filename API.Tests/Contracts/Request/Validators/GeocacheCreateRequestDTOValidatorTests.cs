using API.Contracts.Request;
using API.Contracts.Request.Validators;
using FluentValidation.Results;
using System.Linq;
using Xunit;

namespace API.Tests.Contracts.Request.Validators
{
    public class GeocacheCreateRequestDTOValidatorTests
    {
        [Fact]
        public void GeocacheCreateRequest_HasValidName()
        {
            // Arrange
            GeocacheCreateRequestValidator validator = new();
            const string nullName = null;
            const string blankName = "";
            const string whitespaceName = "   ";
            const string validName = "Test name";
            GeocacheCreateRequestDTO nullNameIsInvalid = new() { Name = nullName };
            GeocacheCreateRequestDTO blankNameIsInvalid = new() { Name = blankName };
            GeocacheCreateRequestDTO whitespaceNameIsInvalid = new() { Name = whitespaceName };
            GeocacheCreateRequestDTO validNameIsValid = new() { Name = validName };


            // Act
            ValidationResult nullNameValidatorResult = validator.Validate(nullNameIsInvalid);
            ValidationResult blankNameValidatorResult = validator.Validate(blankNameIsInvalid);
            ValidationResult whitespaceNameValidatorResult = validator.Validate(whitespaceNameIsInvalid);
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

            Assert.True(validNameValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(validNameIsValid.Name)).Count() == 0
                , $"{nameof(validNameIsValid.Name)} value '{validName}' should be valid.");

        }

        [Fact]
        public void GeocacheCreateRequest_HasValidLatitude()
        {
            // Arrange
            GeocacheCreateRequestValidator validator = new();
            const decimal failingNegativeValue = -91;
            const decimal failingPositiveValue = 91;
            const decimal passingValue = 5.01m;
            GeocacheCreateRequestDTO geocacheLatitudeIsOutsideRange_Negative = new() { Latitude = failingNegativeValue };
            GeocacheCreateRequestDTO geocacheLatitudeIsOutsideRange_Positive = new() { Latitude = failingPositiveValue };
            GeocacheCreateRequestDTO geocacheLatitudeIsValid = new() { Latitude = passingValue }; // Default of 0 is valid.


            // Act
            ValidationResult latitiudeValueNegativeValidatorResult = validator.Validate(geocacheLatitudeIsOutsideRange_Negative);
            ValidationResult latitiudeValuePositiveValidatorResult = validator.Validate(geocacheLatitudeIsOutsideRange_Positive);
            ValidationResult latitudeIsValid = validator.Validate(geocacheLatitudeIsValid);


            // Assert
            Assert.True(latitiudeValueNegativeValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(geocacheLatitudeIsOutsideRange_Negative.Latitude)).Count() > 0
                , $"{nameof(geocacheLatitudeIsOutsideRange_Negative.Latitude)} value '{failingNegativeValue}' should not be valid.");

            Assert.True(latitiudeValuePositiveValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(geocacheLatitudeIsOutsideRange_Positive.Latitude)).Count() > 0
                , $"{nameof(geocacheLatitudeIsOutsideRange_Positive.Latitude)} value '{failingPositiveValue}' should not be valid.");

            Assert.True(latitudeIsValid.Errors
                .Where(e => e.PropertyName == nameof(geocacheLatitudeIsValid.Latitude)).Count() == 0
                , $"{nameof(geocacheLatitudeIsValid.Latitude)} value '{failingPositiveValue}' should be valid.");

        }

        [Fact]
        public void GeocacheCreateRequest_HasValidLongitude()
        {
            // Arrange
            GeocacheCreateRequestValidator validator = new();
            const decimal failingNegativeValue = -181;
            const decimal failingPositiveValue = 181;
            const decimal passingValue = 5.01m;
            GeocacheCreateRequestDTO geocacheLongitudeIsOutsideRange_Negative = new() { Longitude = failingNegativeValue };
            GeocacheCreateRequestDTO geocacheLongitudeIsOutsideRange_Positive = new() { Longitude = failingPositiveValue };
            GeocacheCreateRequestDTO geocacheLongitudeIsValid = new() { Longitude = passingValue }; // Default of 0 is valid.


            // Act
            ValidationResult latitiudeValueNegativeValidatorResult = validator.Validate(geocacheLongitudeIsOutsideRange_Negative);
            ValidationResult latitiudeValuePositiveValidatorResult = validator.Validate(geocacheLongitudeIsOutsideRange_Positive);
            ValidationResult latitudeIsValid = validator.Validate(geocacheLongitudeIsValid);


            // Assert
            Assert.True(latitiudeValueNegativeValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(geocacheLongitudeIsOutsideRange_Negative.Longitude)).Count() > 0
                , $"{nameof(geocacheLongitudeIsOutsideRange_Negative.Longitude)} value {failingNegativeValue} should not be valid.");

            Assert.True(latitiudeValuePositiveValidatorResult.Errors
                .Where(e => e.PropertyName == nameof(geocacheLongitudeIsOutsideRange_Positive.Longitude)).Count() > 0
                , $"{nameof(geocacheLongitudeIsOutsideRange_Positive.Longitude)} value {failingPositiveValue} should not be valid.");

            Assert.True(latitudeIsValid.Errors
                .Where(e => e.PropertyName == nameof(geocacheLongitudeIsValid.Longitude)).Count() == 0
                , $"{nameof(geocacheLongitudeIsValid.Longitude)} value {failingPositiveValue} should be valid.");

        }
    }
}
