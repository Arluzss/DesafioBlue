using App.Domain.Entities;
using App.Features.Commands.UpdateContact;
using App.Interfaces;
using AutoMapper;
using MediatR;
using Moq;

namespace Tests.App.Features.Commands.Update
{
    public class UpdateContactHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Contact_Not_Found()
        {
            var repoMock = new Mock<IContactRepository>();
            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync((Contact?)null);

            var mapperMock = new Mock<IMapper>();
            var handler = new UpdateContactCommandHandler(repoMock.Object, mapperMock.Object);
            var command = new UpdateContactCommand(Guid.NewGuid(), "A", "P@z.com", "444");

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_Should_Map_Update_And_Call_UpdateAsync()
        {
            var existing = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Aaad",
                Email = "daaa@t.com",
                Phone = "999",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var repoMock = new Mock<IContactRepository>();
            repoMock
                .Setup(r => r.GetByIdAsync(existing.Id, CancellationToken.None))
                .ReturnsAsync(existing);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map(It.IsAny<UpdateContactCommand>(), It.IsAny<Contact>()))
                .Callback<UpdateContactCommand, Contact>((command, ct) =>
                {
                    ct.Name = command.Name;
                    ct.Email = command.Email;
                    ct.Phone = command.Phone;
                });

            var handler = new UpdateContactCommandHandler(repoMock.Object, mapperMock.Object);
            var command = new UpdateContactCommand(existing.Id, "BBbb", "Bbb@t.com", "444");

            var before = DateTime.UtcNow;

            var result = await handler.Handle(command, CancellationToken.None);

            mapperMock.Verify(m => m.Map(command, existing), Times.Once);

            Assert.True(existing.CreatedAt >= before,
                "CreatedAt deveria ter sido sobrescrito após o mapeamento");

            repoMock.Verify(r => r.UpdateAsync(
                It.Is<Contact>(c =>
                    c.Id == existing.Id
                 && c.Name == command.Name
                 && c.Email == command.Email
                 && c.Phone == command.Phone
                ),
                CancellationToken.None
            ), Times.Once);

            Assert.Equal(Unit.Value, result);
        }
    }
}
