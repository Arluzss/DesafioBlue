using App.Domain.Entities;
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
            var command = new CreateContactCommand(
                "Arlindo Nunes",
                "arlindo@gmail.com",
                "(81) 91111-1111"
            );
            
            var id = await _handler.Handle(command, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, id);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Contact>(c =>
            c.Name == "Arlindo Nunes" && c.Email == "arlindo@gmail.com")), Times.Once);

        }
        [Fact]
        public async Task Handle_Should_Throw_ValidationException_When_Email_Exists()
        {
            // Arrange
            var command = new CreateContactCommand(
                "Outro Nome",
                "arlindo@gmail.com",
                "(81) 92222-2222"
            );

            _mockRepo
                .Setup(r => r.EmailExistsAsync("arlindo@gmail.com", null))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

    }
}
