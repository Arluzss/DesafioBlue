using App.Domain.Entities;
using App.DTOs;
using App.Features.Commands.CreateContact;
using App.Interfaces;
using App.Mappings;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.App.Features.Commands.Create
{
    public class CreateContactHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _mockRepo;
        private readonly CreateContactCommandHandler _handler;
        private readonly ILoggerFactory _loggerFactory;

        public CreateContactHandlerTests()
        {
            _loggerFactory = LoggerFactory.Create(builder => {});

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContactProfile>();
            }, _loggerFactory);

            _mapper = config.CreateMapper();

            _mockRepo = new Mock<IContactRepository>();

            _handler = new CreateContactCommandHandler(_mockRepo.Object, _mapper);
        }
        [Fact]
        public async Task Handle_Should_Create_Contact_When_Valid()
        {
            var dto = new ContactDto(
                Id: Guid.NewGuid(),       
                Name: "Arlindo Nunes",                
                Email: "email@teste.com", 
                Phone: "(81) 99999-9999"  
            );

            
            var command = new CreateContactCommand(dto);

            var id = await _handler.Handle(command, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);

        }
        [Fact]
        public async Task Handle_Should_Throw_ValidationException_When_Email_Exists()
        {
            var dto = new ContactDto(
                Id: Guid.NewGuid(),
                Name: "Outro Nome",
                Email: "arlindo@gmail.com",
                Phone: "(81) 92222-2222"
            );

            var command = new CreateContactCommand(dto);

            _mockRepo.Setup(r => r.EmailExistsAsync(dto.Email!, It.IsAny<Guid?>()))
                     .ReturnsAsync(true);

            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

    }
}
