using UserManagement.Domain.Entities;

namespace UserManagement.Application.DTO.UserRoleDto
{
    public class JWT
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        
    }

}