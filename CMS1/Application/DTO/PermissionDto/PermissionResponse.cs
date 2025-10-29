namespace UserManagement.Application.DTO.PermissionDto
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string? Description { get; set; }
    }
}
