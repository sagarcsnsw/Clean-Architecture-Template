namespace YourApp.Domain.Entities
{
    public class Hobby
    {
        private string _name;
        
        public int Id { get; set; }
        
        // ✅ Business rule: Hobby name validation
        public string Name 
        { 
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Hobby name must be 1-100 characters");
                _name = value;
            }
        }
        
        public string Category { get; set; }
        public bool IsActive { get; set; }
        
        // ✅ Business logic: Categorization rules
        public bool IsPhysicalActivity() => 
            Category?.ToLower() == "sports" || Category?.ToLower() == "fitness";
    }
}