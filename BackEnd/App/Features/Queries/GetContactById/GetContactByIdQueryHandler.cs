using App.DTOs;
using App.Interfaces;
using AutoMapper;
using MediatR;

namespace App.Features.Queries.GetContactById
{
    public class GetContactByIdHandler
    {
        public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
        {
            private readonly IContactRepository _repository;
            private readonly IMapper _mapper;

            public GetContactByIdQueryHandler(IContactRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken ct)
            {
                var contact = await _repository.GetByIdAsync(request.Id, ct);
                return _mapper.Map<ContactDto>(contact);
            }
        }
    }
}
