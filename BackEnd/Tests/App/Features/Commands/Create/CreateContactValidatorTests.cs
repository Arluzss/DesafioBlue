using App.Features.Commands.CreateContact;
using FluentValidation.TestHelper;

namespace Tests.App.Features.Commands.Create
{
    public class CreateContactValidatorTests
    {
        private readonly CreateContactValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateContactCommand("", "email@teste.com", "(81) 99999-9999");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("O nome é obrigatório");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Too_Long()
        {
            var longName = new string('A', 101);
            var command = new CreateContactCommand(longName, "email@teste.com", "(81) 99999-9999");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("O nome deve conter no máximo 100 caracteres");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var command = new CreateContactCommand("Nome", "", "(81) 99999-9999");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("O e-mail é obrigatório");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Invalid()
        {
            var command = new CreateContactCommand("Nome", "invalido", "(81) 99999-9999");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("o e-mail é inválido");
        }

        [Fact]
        public void Should_Have_Error_When_Phone_Is_Empty()
        {
            var command = new CreateContactCommand("Nome", "email@teste.com", "");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                  .WithErrorMessage("O número do telefone é obrigatório");
        }

        [Fact]
        public void Should_Have_Error_When_Phone_Invalid_Format()
        {
            var command = new CreateContactCommand("Nome", "email@teste.com", "12345");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                  .WithErrorMessage("Formato inválido (ex: (81) 9343-3456)"); 
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new CreateContactCommand("Nome", "email@teste.com", "(81) 91234-5678");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
