using YourApp.Domain.Entities;
using YourApp.Infrastructure.Data.Models;

namespace YourApp.Infrastructure.Mappings
{
    public static class UserMappingExtensions
    {
        // ✅ Entity mapping between data models and Core entities
        public static User ToDomainEntity(this UserEntity entity)
        {
            if (entity == null) return null;
            
            return new User
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.EmailAddress,
                CreatedAt = entity.CreatedDate,
                IsActive = !entity.IsDeleted,
                ParentId = entity.ParentId
            };
        }
        
        public static UserEntity ToDataEntity(this User domain)
        {
            if (domain == null) return null;
            
            return new UserEntity
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                EmailAddress = domain.Email,
                CreatedDate = domain.CreatedAt,
                IsDeleted = !domain.IsActive,
                ParentId = domain.ParentId
            };
        }
    }
}