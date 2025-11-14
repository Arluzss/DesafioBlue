using App.Domain.Entities;
using App.DTOs;
using App.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;


namespace App.Features.Commands.CreateContact
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Guid>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public CreateContactCommandHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateContactCommand request, CancellationToken ct)
        {
            if (await _repository.EmailExistsAsync(request.ContactDto.Email!))
            {
                throw new ValidationException("E-mail já cadastrado.");
            }
            var contact = _mapper.Map<Contact>(request.ContactDto);
            
            await _repository.AddAsync(contact);

            return contact.Id;
        }
    }
}