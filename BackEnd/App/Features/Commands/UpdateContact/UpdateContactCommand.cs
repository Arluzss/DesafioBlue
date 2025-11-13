using MediatR;

namespace App.Features.Commands.UpdateContact
{
   public record UpdateContactCommand(Guid Id, string Name, string Email, string Phone) : IRequest<Unit>;
}
