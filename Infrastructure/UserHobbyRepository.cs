using Microsoft.EntityFrameworkCore;
using YourApp.Application.DTOs;
using YourApp.Application.Interfaces;
using YourApp.Infrastructure.Data;

namespace YourApp.Infrastructure.Repositories
{
    public class UserHobbyRepository : IUserHobbyRepository
    {
        private readonly ApplicationDbContext _context;

        public UserHobbyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Forms DTOs and sends to Application layer
        public async Task<List<UserWithHobbiesDto>> GetUsersWithHobbiesAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .Include(u => u.UserHobbies)
                .ThenInclude(uh => uh.Hobby)
                .Select(u => new UserWithHobbiesDto
                {
                    UserId = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    Email = u.EmailAddress,
                    CreatedAt = u.CreatedDate,
                    IsActive = !u.IsDeleted,
                    UserType = u.ParentId.HasValue ? "Student" : "Parent",
                    TotalHobbies = u.UserHobbies.Count(),
                    Hobbies = u.UserHobbies.Select(uh => new UserHobbyDto
                    {
                        HobbyId = uh.Hobby.Id,
                        HobbyName = uh.Hobby.Name,
                        Category = uh.Hobby.Category,
                        SkillLevel = uh.SkillLevel,
                        ExperienceMonths = (int)(DateTime.UtcNow - uh.StartedAt).TotalDays / 30,
                        IsPhysicalActivity = uh.Hobby.Category.ToLower() == "sports" || 
                                           uh.Hobby.Category.ToLower() == "fitness"
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<UserWithHobbiesDto> GetUserWithHobbiesByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.UserHobbies)
                .ThenInclude(uh => uh.Hobby)
                .Select(u => new UserWithHobbiesDto
                {
                    UserId = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    Email = u.EmailAddress,
                    CreatedAt = u.CreatedDate,
                    IsActive = !u.IsDeleted,
                    UserType = u.ParentId.HasValue ? "Student" : "Parent",
                    TotalHobbies = u.UserHobbies.Count(),
                    Hobbies = u.UserHobbies.Select(uh => new UserHobbyDto
                    {
                        HobbyId = uh.Hobby.Id,
                        HobbyName = uh.Hobby.Name,
                        Category = uh.Hobby.Category,
                        SkillLevel = uh.SkillLevel,
                        ExperienceMonths = (int)(DateTime.UtcNow - uh.StartedAt).TotalDays / 30,
                        IsPhysicalActivity = uh.Hobby.Category.ToLower() == "sports" || 
                                           uh.Hobby.Category.ToLower() == "fitness"
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddUserHobbyAsync(int userId, int hobbyId, int skillLevel)
        {
            var userHobby = new UserHobbyEntity
            {
                UserId = userId,
                HobbyId = hobbyId,
                SkillLevel = skillLevel,
                StartedAt = DateTime.UtcNow
            };

            _context.UserHobbies.Add(userHobby);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserHobbyAsync(int userId, int hobbyId)
        {
            var userHobby = await _context.UserHobbies
                .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HobbyId == hobbyId);

            if (userHobby != null)
            {
                _context.UserHobbies.Remove(userHobby);
                await _context.SaveChangesAsync();
            }
        }
    }
}