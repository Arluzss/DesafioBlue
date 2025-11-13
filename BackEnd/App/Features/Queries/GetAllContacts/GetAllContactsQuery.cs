using App.DTOs;
using MediatR;

namespace App.Features.Queries.GetAllContacts
{
    public record GetAllContactsQuery() : IRequest<List<ContactDto>>;
}
