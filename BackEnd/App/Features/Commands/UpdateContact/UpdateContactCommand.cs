using App.DTOs;
using MediatR;

namespace App.Features.Commands.UpdateContact
{
   public record UpdateContactCommand(ContactDto ContactDto) : IRequest<Unit>;
}
