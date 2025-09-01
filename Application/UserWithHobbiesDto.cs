namespace YourApp.Application.DTOs
{
    // ✅ DTO defined in Application layer for view requirements
    public class UserWithHobbiesDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<UserHobbyDto> Hobbies { get; set; } = new();
        public int TotalHobbies { get; set; }
        public string UserType { get; set; } // Student/Parent
    }

    public class UserHobbyDto
    {
        public int HobbyId { get; set; }
        public string HobbyName { get; set; }
        public string Category { get; set; }
        public int SkillLevel { get; set; }
        public int ExperienceMonths { get; set; }
        public bool IsPhysicalActivity { get; set; }
    }

    public class CreateUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? ParentId { get; set; }
        public List<int> HobbyIds { get; set; } = new();
    }
}