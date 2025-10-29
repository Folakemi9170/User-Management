namespace UserManagement.Domain.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int? EmployeeId { get; set; }
        //Navigation Property
        public Employee Employee { get; set; }
        public string? RoleName { get; set; }

    }
}