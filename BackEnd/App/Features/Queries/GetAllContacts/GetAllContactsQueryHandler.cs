using App.DTOs;
using App.Interfaces;
using AutoMapper;
using MediatR;

namespace App.Features.Queries.GetAllContacts
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, List<ContactDto>>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GetAllContactsQueryHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken ct)
        {
            var contacts = await _repository.GetAllAsync(ct);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
    }
}
