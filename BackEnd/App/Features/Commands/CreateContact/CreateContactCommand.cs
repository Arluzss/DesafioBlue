using App.DTOs;
using MediatR;

namespace App.Features.Commands.CreateContact
{
    public record CreateContactCommand(ContactDto ContactDto) : IRequest<Guid>;
}
