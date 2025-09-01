namespace YourApp.Domain.Entities
{
    public class User
    {
        private string _firstName;
        private string _email;

        public int Id { get; set; }
        
        // ✅ Business rule: Validation in Core layer
        public string FirstName 
        { 
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                    throw new ArgumentException("First name must be 1-50 characters");
                _firstName = value;
            }
        }
        
        public string LastName { get; set; }
        
        // ✅ Business rule: Email validation
        public string Email 
        { 
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Valid email is required");
                _email = value;
            }
        }
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; } // For student-parent relationship

        // ✅ Business logic: Reusable across projects
        public string GetFullName() => $"{FirstName} {LastName}".Trim();
        
        public bool CanHaveHobbies() => IsActive && Id > 0;
        
        // ✅ Business rule: Student updates when parent changes
        public void UpdateFromParent(User parent)
        {
            if (parent == null || parent.Id == this.Id) return;
            
            // Business logic: Students inherit some parent properties
            if (this.ParentId == parent.Id)
            {
                // Update student information based on parent changes
                this.CreatedAt = DateTime.UtcNow; // Track when update happened
            }
        }
    }
}