namespace UserManagement.Application.DTO.EmployeeDto
{
    public class UpdateEmployee
    {
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
    }
}
