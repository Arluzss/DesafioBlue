using App.Interfaces;
using MediatR;

namespace App.Features.Commands.DeleteContact
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(IContactRepository repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken ct)
        {
            var contact = await _repository.GetByIdAsync(request.Id, ct);
            if (contact == null)
            {
                throw new KeyNotFoundException("Contato não encontrado.");
            }
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }

    }
}
