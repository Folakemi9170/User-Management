using UserManagement.Application.DTO.DepartmentDto;
namespace UserManagement.Application.Interfaces
{
    public interface IDeptService
    {
        Task<ResponseDto> CreateDepartment(CreateDeptDto department);
        Task<DepartmentResponseDto> GetDepartment(int id);
        Task<DepartmentResponseDto> GetAllDepartments();
        //Task<DepartmentResponseDto> Update(int id, UpdateDept department);
        Task<DepartmentResponseDto> Delete(int id);
    }
}
