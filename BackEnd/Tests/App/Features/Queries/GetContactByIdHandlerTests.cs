using App.Domain.Entities;
using App.Features.Queries.GetAllContacts;
using App.Features.Queries.GetContactById;
using App.Interfaces;
using App.Mappings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata;
using static App.Features.Queries.GetContactById.GetContactByIdHandler;

namespace Tests.App.Features.Queries
{
    public class GetContactByIdHandlerTests
    {

        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _mockRepo;
        private readonly GetContactByIdQueryHandler _handler;
        private readonly ILoggerFactory _loggerFactory;

        public GetContactByIdHandlerTests()
        {
            _loggerFactory = LoggerFactory.Create(builder => { });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContactProfile>();
            }, _loggerFactory);

            _mapper = config.CreateMapper();

            _mockRepo = new Mock<IContactRepository>();

            _handler = new GetContactByIdQueryHandler(_mockRepo.Object, _mapper);
        }
        [Fact]
        public async Task Handle_Should_Return_Null_When_Contact_Not_Found()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Contact?)null);

            var result = await _handler.Handle(new GetContactByIdQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_Dto_When_Contact_Found()
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Simmba",
                Email = "Simba@selva.com",
                Phone = "222",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.GetByIdAsync(contact.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contact);

            var result = await _handler.Handle(new GetContactByIdQuery(contact.Id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(contact.Id, result.Id);
            Assert.Equal(contact.Name, result.Name);
            Assert.Equal(contact.Email, result.Email);
            Assert.Equal(contact.Phone, result.Phone);
        }
    }
}
