using Microsoft.AspNetCore.Http;
using YourApp.Application.Interfaces;
using YourApp.Domain.Entities;
using System.Security.Claims;

namespace YourApp.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // ✅ External system access
        private readonly IUserRepository _userRepository;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        // ✅ Communication with HttpContext
        public async Task<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            return await _userRepository.GetByIdAsync(userId);
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException("User not authenticated");

            return userId;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}