using Xunit;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Restaurant.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test", 
                Category = "Italian",
                ContactEmail = "test@gmail.com",
                PostalCode = "12-345"
            };

            var validator = new CreateRestaurantCommandValidator();

            // act
            var result = validator.TestValidate(command);
        
            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validator_ForInValidCommand_ShouldHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Te",
                Category = "Ita",
                ContactEmail = "@gmail",
                PostalCode = "12345"
            };

            var validator = new CreateRestaurantCommandValidator();

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);

        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Indian")]
        [InlineData("Mexican")]
        [InlineData("Japanese")]
        [InlineData("American")]

        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand() { Category = category };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);

        }

        [Theory()]
        [InlineData("10220")]
        [InlineData("102-20")]
        [InlineData("12 202")]
        [InlineData("10-2 10")]
        public void Validator_ForValidPostalCode_ShouldNotHaveValidationErrorsForPostalCodeProperty(string postalCode)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand() { PostalCode = postalCode };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);

        }
    }
}