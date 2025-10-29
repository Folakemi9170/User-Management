namespace UserManagement.Application.DTO.PermissionDto
{
    public class CreatePermissionDto
    {
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string? Description { get; set; }
    }
}
