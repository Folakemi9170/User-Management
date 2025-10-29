namespace UserManagement.Application.DTO.EmployeeDto.Paging
{
    public class Filter
    {
        public string? Gender { get; set; } = null;
        public int? DepartmentId { get; set; } = null;
        public bool? IsActive { get; set; } = null;
    }
}
