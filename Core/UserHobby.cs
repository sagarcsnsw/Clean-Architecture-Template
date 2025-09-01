namespace YourApp.Domain.Entities
{
    public class UserHobby
    {
        public int UserId { get; set; }
        public int HobbyId { get; set; }
        public DateTime StartedAt { get; set; }
        public int SkillLevel { get; set; } // 1-10
        
        // ✅ Business rule: Skill level validation
        public void SetSkillLevel(int level)
        {
            if (level < 1 || level > 10)
                throw new ArgumentException("Skill level must be between 1 and 10");
            SkillLevel = level;
        }
        
        // ✅ Business logic: Experience calculation
        public int GetExperienceMonths() => 
            (int)(DateTime.UtcNow - StartedAt).TotalDays / 30;
    }
}