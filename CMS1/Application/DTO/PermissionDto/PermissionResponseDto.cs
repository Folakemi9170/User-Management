namespace UserManagement.Application.DTO.PermissionDto
{
    public class PermissionResponseDto
    {
        public string Message { get; set; }
        public IEnumerable<PermissionResponse> Permissions { get; set; }
    }
}
