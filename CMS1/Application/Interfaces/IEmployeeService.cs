using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.EmployeeDto.Paging;

namespace UserManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse> CreateEmployee(CreateEmployee employee);
        Task<EmployeeResponse> GetEmployeesById(int id);
        Task<PagedResponse<EmployeeResponse>> GetAllEmployees(Filter filter = null, Pagination pagination = null);
        Task<EmployeeResponse> UpdateEmployee(int id, UpdateEmployee employee);
        Task<EmployeeResponse> PatchEmployee(int id, PartialUpdate employeedto);

    }
}
