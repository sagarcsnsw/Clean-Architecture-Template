using YourApp.Domain.Entities;

namespace YourApp.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Task<User> GetCurrentUserAsync();
        int GetCurrentUserId();
        bool IsAuthenticated();
    }
}