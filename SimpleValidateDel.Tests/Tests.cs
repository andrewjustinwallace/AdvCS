using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValidateDel.Tests
{
    [TestFixture]
    public class RegistrationValidatorTests
    {
        private RegistrationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RegistrationValidator();
        }

        [Test]
        public void Validate_WithValidInput_ReturnsTrue()
        {
            // Arrange
            _validator.AddValidation(input => !string.IsNullOrEmpty(input));
            _validator.AddValidation(input => input.Length >= 8);
            _validator.AddValidation(input => input.Any(char.IsUpper));

            // Act
            bool result = _validator.Validate("Password123");

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Validate_WithInvalidInput_ReturnsFalse()
        {
            // Arrange
            _validator.AddValidation(input => !string.IsNullOrEmpty(input));
            _validator.AddValidation(input => input.Length >= 8);
            _validator.AddValidation(input => input.Any(char.IsUpper));

            // Act
            bool result = _validator.Validate("password");

            // Assert
            Assert.That(!result);
        }

        [Test]
        public void Validate_WithNoValidations_ReturnsTrue()
        {
            // Act
            bool result = _validator.Validate("AnyInput");

            // Assert
            Assert.That(result);
        }

        [Test]
        public void AddValidation_AddsValidationToList()
        {
            // Arrange
            int validationCount = 0;
            _validator.AddValidation(_ => { validationCount++; return true; });

            // Act
            _validator.Validate("AnyInput");

            // Assert
            Assert.That(1 == validationCount);
        }
    }

    // The RegistrationValidator class for reference
    public delegate bool ValidationDelegate(string input);

    public class RegistrationValidator
    {
        private readonly List<ValidationDelegate> _validations = new List<ValidationDelegate>();

        public void AddValidation(ValidationDelegate validation)
        {
            _validations.Add(validation);
        }

        public bool Validate(string input)
        {
            return _validations.All(validation => validation(input));
        }
    }
}
