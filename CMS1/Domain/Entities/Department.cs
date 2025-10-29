using UserManagement.Domain.Entities.Product;

namespace UserManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string DeptName { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>(); // One department can have many employees
        public ICollection<JobTitle> JobTitles { get; set; } // One department can have many job titles
        public ICollection<Products> Products { get; set; }
    }
}
