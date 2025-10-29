namespace UserManagement.Domain.Entities
{
    public class Roles: BaseEntity
    {
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Description { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<JobTitle> JobTitles { get; set; }
    }
}
