using App.DTOs;
using MediatR;

namespace App.Features.Queries.GetContactById
{
    public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto>;
}
