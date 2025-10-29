using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class DeptService : IDeptService
    {
        private readonly IGenericRepository<Department> _departmentRepo;
        private readonly UMSDbContext _dbContext;
        private readonly IMapper _mapper;


        public DeptService(IGenericRepository<Department> departmentRepo, UMSDbContext dbContext, IMapper mapper)
        {
            _departmentRepo = departmentRepo;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto> CreateDepartment(CreateDeptDto department)
        {
            var dto = new Department
            {
                DeptName = department.DeptName,
                CreatedAt = department.CreatedAt
            };

            var created = await _dbContext.AddAsync(dto);
            await _dbContext.SaveChangesAsync();


            return _mapper.Map<ResponseDto>(created.Entity);
        }

        public async Task<DepartmentResponseDto> GetAllDepartments()
        {
            var departments = await _departmentRepo.GetAll();
            if (departments == null || !departments.Any())
            {
                return new DepartmentResponseDto
                {
                    Message = "No departments found",
                    Departments = new List<ResponseDto>()
                };
            }

            return new DepartmentResponseDto
            {
                Message = "Departments retrieved successfully",
                Departments = _mapper.Map<IEnumerable<ResponseDto>>(departments)
            };
        }

        public async Task<DepartmentResponseDto> GetDepartment(int id)
        {
            var department = _departmentRepo.Get(id);
            if (department == null)
            {
                return new DepartmentResponseDto
                {
                    Message = $"Department with id {id} not found",
                    Departments = new List<ResponseDto>()
                };
            }

            return new DepartmentResponseDto
            {
                Message = "Department retrieved successfully",
                Departments = new List<ResponseDto>
                {
                    _mapper.Map<ResponseDto>(department)
                }
            };
        }

        public async Task<DepartmentResponseDto> Delete(int id)
        {
            var department = _departmentRepo.Get(id);

            if (department == null)
            {
                return new DepartmentResponseDto
                {
                    Message = $"Department with id {id} not found",
                    Departments = new List<ResponseDto>()
                };
            }

            _departmentRepo.Delete(id);

            return new DepartmentResponseDto
            {
                Message = "Department deleted successfully",
                Departments = new List<ResponseDto>
                {
                    _mapper.Map<ResponseDto>(department)
                }
            };
        }
    }
}
