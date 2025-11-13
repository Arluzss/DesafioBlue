using App.Domain.Entities;

namespace App.Interfaces
{
    public interface IContactRepository
    {
        Task<Guid> AddAsync(Contact contact);
        Task<List<Contact>> GetAllAsync(CancellationToken ct = default);
        Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task UpdateAsync(Contact contact, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, Guid? ignoreId = null);

    }
}
