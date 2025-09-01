using System.ComponentModel.DataAnnotations.Schema;

namespace YourApp.Infrastructure.Data.Models
{
    // ✅ EF data model - stays in Infrastructure
    [Table("Users")]
    public class UserEntity
    {
        public int Id { get; set; }
        
        [Column("FirstName")]
        public string FirstName { get; set; }
        
        [Column("LastName")]
        public string LastName { get; set; }
        
        [Column("EmailAddress")]
        public string EmailAddress { get; set; }
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
        
        [Column("ParentId")]
        public int? ParentId { get; set; }

        // EF Navigation properties
        public virtual ICollection<UserHobbyEntity> UserHobbies { get; set; }
        public virtual UserEntity Parent { get; set; }
        public virtual ICollection<UserEntity> Children { get; set; }
    }

    [Table("Hobbies")]
    public class HobbyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("UserHobbies")]
    public class UserHobbyEntity
    {
        public int UserId { get; set; }
        public int HobbyId { get; set; }
        public DateTime StartedAt { get; set; }
        public int SkillLevel { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual HobbyEntity Hobby { get; set; }
    }
}