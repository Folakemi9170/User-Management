using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTO.JobTitleDto
{
    public class JobTitleResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinimumSalary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaximumSalary { get; set; }
    }
}
