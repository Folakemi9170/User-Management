using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTO.JobTitleDto
{
    public class CreateJobTitle 
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
    }
}
