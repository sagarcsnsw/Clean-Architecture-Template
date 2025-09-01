using Microsoft.EntityFrameworkCore;
using YourApp.Application.Interfaces;
using YourApp.Domain.Entities;
using YourApp.Infrastructure.Data;
using YourApp.Infrastructure.Mappings;

namespace YourApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Implements Application interface
        public async Task<User> GetByIdAsync(int id)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
                
            return entity.ToDomainEntity(); // ✅ Map to Core entity
        }

        public async Task<List<User>> GetAllActiveUsersAsync()
        {
            var entities = await _context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
                
            return entities.Select(e => e.ToDomainEntity()).ToList();
        }

        public async Task<User> CreateAsync(User user)
        {
            var entity = user.ToDataEntity(); // ✅ Map from Core entity
            
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            
            return entity.ToDomainEntity();
        }

        public async Task<User> UpdateAsync(User user)
        {
            var entity = await _context.Users.FindAsync(user.Id);
            if (entity == null) return null;

            var updatedEntity = user.ToDataEntity();
            entity.FirstName = updatedEntity.FirstName;
            entity.LastName = updatedEntity.LastName;
            entity.EmailAddress = updatedEntity.EmailAddress;
            entity.ParentId = updatedEntity.ParentId;

            await _context.SaveChangesAsync();
            return entity.ToDomainEntity();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsersByParentIdAsync(int parentId)
        {
            var entities = await _context.Users
                .Where(u => u.ParentId == parentId && !u.IsDeleted)
                .ToListAsync();
                
            return entities.Select(e => e.ToDomainEntity()).ToList();
        }
    }
}