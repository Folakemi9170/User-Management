using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Domain.Entities.Product;

namespace UserManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string Firstname { get; set; }
        public string? Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DOB { get; set; }
        public bool IsActive { get; set; } = true;



        //Navigation Property
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }

        public int? JobTitleId { get; set; }
        [ForeignKey("JobTitleId")]
        public JobTitle JobTitle { get; set; }

        public ICollection<Products> CreatedProducts { get; set; }
        public ICollection<Products> AssignedProducts { get; set; }

    }
}
