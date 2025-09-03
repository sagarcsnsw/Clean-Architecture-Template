using YourApp.Application.DTOs;

namespace YourApp.Application.Interfaces
{
    public interface IUserHobbyRepository
    {
        Task<List<UserWithHobbiesDto>> GetUsersWithHobbiesAsync();

        Task<UserWithHobbiesDto> GetUserWithHobbiesByIdAsync(int userId);

        Task AddUserHobbyAsync(int userId, int hobbyId, int skillLevel);

        Task RemoveUserHobbyAsync(int userId, int hobbyId);
    }
}