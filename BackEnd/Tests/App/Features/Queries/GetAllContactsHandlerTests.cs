using App.Domain.Entities;
using App.Features.Queries.GetAllContacts;
using App.Interfaces;
using App.Mappings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.App.Features.Queries
{
    public class GetAllContactsHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _mockRepo;
        private readonly GetAllContactsQueryHandler _handler;
        private readonly ILoggerFactory _loggerFactory;

        public GetAllContactsHandlerTests()
        {
            _loggerFactory = LoggerFactory.Create(builder => { });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContactProfile>();
            }, _loggerFactory);

            _mapper = config.CreateMapper();

            _mockRepo = new Mock<IContactRepository>();

            _handler = new GetAllContactsQueryHandler(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Contacts()
        {
            
            _mockRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Contact>());

            var result = await _handler.Handle(new GetAllContactsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_List_When_Contacts_Exist()
        {
            var contacts = new List<Contact>
            {
                new Contact { Id = Guid.NewGuid(), Name = "Hanna", Email = "Hanna@nana.com", Phone = "000", CreatedAt = DateTime.UtcNow },
                new Contact { Id = Guid.NewGuid(), Name = "Simba", Email = "Simba@selva.com", Phone = "111", CreatedAt = DateTime.UtcNow }
            };
            _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contacts);

            var result = await _handler.Handle(new GetAllContactsQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, dto => dto.Id == contacts[0].Id && dto.Name == "Hanna" && dto.Email == "Hanna@nana.com" && dto.Phone == "000");
            Assert.Contains(result, dto => dto.Id == contacts[1].Id && dto.Name == "Simba" && dto.Email == "Simba@selva.com" && dto.Phone == "111");
        }

    }
}
