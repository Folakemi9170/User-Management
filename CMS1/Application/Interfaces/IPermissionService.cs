using UserManagement.Application.DTO.PermissionDto;

namespace UserManagement.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionResponse> CreatePermission(CreatePermissionDto dto);
        Task<PermissionResponseDto> GetAllPermission();
        Task<PermissionResponseDto> DeletePermission(int id);
    }
}
