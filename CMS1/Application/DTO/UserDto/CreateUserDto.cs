using UserManagement.Domain.Entities;

namespace UserManagement.Application.DTO.UserDto
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}