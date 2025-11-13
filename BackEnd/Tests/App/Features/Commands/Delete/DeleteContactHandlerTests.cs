using App.Domain.Entities;
using App.Features.Commands.DeleteContact;
using App.Interfaces;
using MediatR;
using Moq;

namespace Tests.App.Features.Commands.Delete
{
    public class DeleteContactHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteAsync_And_Return_Unit()
        {
            var repoMock = new Mock<IContactRepository>();
            var fakeId = Guid.NewGuid();

            repoMock.Setup(r => r.GetByIdAsync(fakeId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Contact { Id = fakeId });

            var handler = new DeleteContactCommandHandler(repoMock.Object);
            var command = new DeleteContactCommand(fakeId);

            var result = await handler.Handle(command, CancellationToken.None);

            repoMock.Verify(r => r.DeleteAsync(fakeId, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }
    }
}
