
using Infrastructure.Context;
using App.Interfaces;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Guid> AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var contact = await _context.Contacts.FindAsync(new object[] { id }, ct);

            if (contact != null) { 
                _context.Contacts.Remove(contact); 
            }

            await _context.SaveChangesAsync(ct);
        }

        public Task<bool> EmailExistsAsync(string email, Guid? ignoreId = null)
        {
            return _context.Contacts
            .AnyAsync(c => c.Email == email && (!ignoreId.HasValue || c.Id != ignoreId.Value));
        }

        public async Task<List<Contact>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Contacts.ToListAsync(ct);
        }

        public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Contacts.FindAsync(id, ct);
        }

        public async Task UpdateAsync(Contact contact, CancellationToken ct)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
        }
    }
}
