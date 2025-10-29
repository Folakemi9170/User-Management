using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.UserDto;
using UserManagement.Application.DTO.UserRoleDto;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserDto user);
        Task<UserLoginResponse> Login(UserLoginDto loginmodel);
        Task<CreateUserResponse> UpdateUser(int id, UpdateUserDto dto);
    }
}
