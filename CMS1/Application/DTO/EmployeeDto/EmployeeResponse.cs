namespace UserManagement.Application.DTO.EmployeeDto
{
    public class EmployeeResponse
    {
            public string Fullname { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateOnly DOB { get; set; }
            public string RoleName { get; set; }
            public string? DeptName { get; set; }
            public bool IsActive { get; set; }
            public string JobTitleName { get; set; }
        }
    }