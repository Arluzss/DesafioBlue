using App.Features.Commands.UpdateContact;
using App.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace Tests.App.Features.Commands.Update
{
    public class UpdateContactValidatorTests
    {
        private readonly Mock<IContactRepository> _repoMock;
        private readonly UpdateContactValidator _validator;

        public UpdateContactValidatorTests()
        {
            _repoMock = new Mock<IContactRepository>();
            _validator = new UpdateContactValidator(_repoMock.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Already_In_Use()
        {
            // Arrange
            _repoMock.Setup(r => r.EmailExistsAsync("existe@teste.com", It.IsAny<Guid>()))
                     .ReturnsAsync(true);

            var command = new UpdateContactCommand(
                Guid.NewGuid(),
                "Arley",
                "existe@teste.com", // e-mail duplicado
                "(11) 91234-5678"
            );

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("E-mail já está em uso");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                     .ReturnsAsync(false);

            var command = new UpdateContactCommand(
                Guid.NewGuid(),
                "Arlindo",
                "nordeste@teste.com",
                "(11) 92345-6789"
            );

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
