using YourApp.Domain.Entities;

namespace YourApp.Application.Interfaces
{
    // ✅ Interface definition (port) for Infrastructure
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetAllActiveUsersAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<List<User>> GetUsersByParentIdAsync(int parentId);
    }
}