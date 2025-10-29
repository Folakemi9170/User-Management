using UserManagement.Application.DTO.PermissionDto;

namespace UserManagement.Application.DTO.Roles
{
    public class PermissionRoleResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<PermissionResponse> Permissions { get; set; }
    }
}
