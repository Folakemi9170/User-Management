namespace UserManagement.Domain.Entities
{
    public class JobTitle : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int? RoleId { get; set; }
        public Roles Role { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }

        public bool IsActive { get; set; } = true;
    }
}


//Job Titles: “What” the employee does