namespace UserManagement.Application.DTO.Roles
{
    public class UpdateRole
    {
        public string? RoleName { get; set; }
        public string? Description { get; set; }

        public List<int> PermissionId { get; set; } 
    }
}
