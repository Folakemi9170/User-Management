using System.Threading.Tasks;
using UserManagement.Application.DTO.Roles;

namespace UserManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDto> CreateRole(CreateRoleDto role);
        public Task<List<PermissionRoleResponse>> GetAllRoles();
        Task<PermissionRoleResponse> GetPermissionByRoleId(int id);
        Task AssignPermissionsToRole(int roleId, List<int> permissionIds);
        Task<PermissionRoleResponse> UpdateRolePermission(int Id, UpdateRole dto);   
            }
}
