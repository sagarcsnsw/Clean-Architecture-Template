using YourApp.Application.DTOs;
using YourApp.Application.Interfaces;
using YourApp.Domain.Entities;

namespace YourApp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHobbyRepository _userHobbyRepository;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            IUserRepository userRepository,
            IUserHobbyRepository userHobbyRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _userHobbyRepository = userHobbyRepository;
            _currentUserService = currentUserService;
        }

        // ✅ Use case: Get users with hobbies for reporting
        public async Task<List<UserWithHobbiesDto>> GetUsersWithHobbiesAsync()
        {
            // ✅ Argument validation in Application layer
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (!currentUser.IsActive)
                throw new UnauthorizedAccessException("Only active users can view user list");

            // ✅ Call Infrastructure interface
            return await _userHobbyRepository.GetUsersWithHobbiesAsync();
        }

        // ✅ Use case: Create user with business logic
        public async Task<User> CreateUserAsync(CreateUserRequestDto request)
        {
            // ✅ Argument validation
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new ArgumentException("First name is required");

            // ✅ Create Core entity (validation happens in entity)
            var user = new User
            {
                FirstName = request.FirstName, // Validation in Core layer
                LastName = request.LastName,
                Email = request.Email, // Validation in Core layer
                ParentId = request.ParentId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // ✅ Business logic: Handle parent-child relationship
            if (request.ParentId.HasValue)
            {
                var parent = await _userRepository.GetByIdAsync(request.ParentId.Value);
                if (parent != null)
                {
                    user.UpdateFromParent(parent); // Core business logic
                }
            }

            // ✅ Call Infrastructure to persist
            var createdUser = await _userRepository.CreateAsync(user);

            // ✅ Business workflow: Add hobbies if specified
            foreach (var hobbyId in request.HobbyIds)
            {
                await _userHobbyRepository.AddUserHobbyAsync(createdUser.Id, hobbyId, 1);
            }

            return createdUser;
        }

        // ✅ Use case: Update student when parent changes
        public async Task UpdateStudentsWhenParentChangesAsync(int parentId)
        {
            // ✅ Business workflow orchestration
            var parent = await _userRepository.GetByIdAsync(parentId);
            if (parent == null) return;

            var students = await _userRepository.GetUsersByParentIdAsync(parentId);
            
            foreach (var student in students)
            {
                // ✅ Core business logic
                student.UpdateFromParent(parent);
                await _userRepository.UpdateAsync(student);
            }
        }
    }
}