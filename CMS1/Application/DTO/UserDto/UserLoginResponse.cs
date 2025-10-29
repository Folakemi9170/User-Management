using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.PermissionDto;

namespace UserManagement.Application.DTO.UserDto
{
    public class UserLoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool IsEmployee { get; set; } = true;
        public EmployeeResponse Employee { get; set;}
        public IEnumerable<PermissionResponse> Permissions { get; set; }
    }
}
