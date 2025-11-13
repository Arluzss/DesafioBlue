using MediatR;

namespace App.Features.Commands.DeleteContact
{
    public record DeleteContactCommand(Guid Id) : IRequest<Unit>;
}
