using MediatR;

namespace App.Features.Commands.CreateContact
{
    public record CreateContactCommand(string Name, string Email, string Phone) : IRequest<Guid>;
}
