using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.EmployeeDto.Paging;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class EmployeeService : IEmployeeService

    {
        private readonly UMSDbContext _dbContext;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public EmployeeService(UMSDbContext dbContext, IGenericRepository<Employee> employeeRepo, IMapper mapper, IMemoryCache cache, IEmailService emailService)
        {
            _dbContext = dbContext;
            _employeeRepo = employeeRepo;
            _mapper = mapper;
            _cache = cache;
            _emailService = emailService;
        }

        public async Task<EmployeeResponse> CreateEmployee(CreateEmployee dto)
        {
            bool exists = _dbContext.Employees.Any(x => x.Email == dto.Email);

            if (exists)
                throw new ArgumentException("Employee already exists.");


            var employees = _mapper.Map<Employee>(dto);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user != null)
            {
                employees.UserId = user.Id;
            }

            await _dbContext.Employees.AddAsync(employees);
            await _dbContext.SaveChangesAsync();

            Task.Run(() => _emailService.SendProfileCompletionEmail(employees.Email, employees.Firstname));

            var createdEmployee = await _dbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.JobTitle)
                .FirstOrDefaultAsync(e => e.Id == employees.Id);

            return _mapper.Map<EmployeeResponse>(employees);
        }


        public async Task<EmployeeResponse> GetEmployeesById(int id)
        {
            string cacheKey = $"employee_{id}";
            if (_cache.TryGetValue(cacheKey, out EmployeeResponse cachedEmployee))
            {
                Log.Information("Fetching employee {EmployeeId} from cache", id);
                return cachedEmployee;
            }

            // Only logs if cache miss (so DB is queried)
            Log.Information("Fetching employee {EmployeeId} from database", id);

            var employee = await _dbContext.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeResponse
                {
                    Fullname = $"{e.Firstname} {e.Middlename} {e.Lastname}",
                    Gender = e.Gender,
                    Email = e.Email,
                    Phone = e.Phone,
                    DOB = e.DOB,
                    RoleName = e.Role.RoleName,
                    DeptName = e.Department.DeptName,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();

            if (employee != null)
            {
                _cache.Set(cacheKey, employee, TimeSpan.FromMinutes(5));
            }

            return employee;
        }

        public async Task<PagedResponse<EmployeeResponse>> GetAllEmployees(Filter filter = null, Pagination pagination = null)
        {
            var query = _dbContext.Employees
                .Include(e => e.Department)   // bring in department
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Gender))
                    query = query.Where(e => e.Gender == filter.Gender);

                if (filter.DepartmentId != null)
                    query = query.Where(e => e.DepartmentId == filter.DepartmentId);

                if (filter.IsActive.HasValue)
                    query = query.Where(e => e.IsActive == filter.IsActive.Value);
            }

            var totalRecords = await query.CountAsync();
            var pageNumber = pagination?.PageNumber ?? 1;
            var pageSize = pagination?.PageSize ?? 10;

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeResponse
                {
                    Fullname = $"{e.Firstname} {e.Middlename} {e.Lastname}",
                    Gender = e.Gender,
                    Email = e.Email,
                    Phone = e.Phone,
                    DOB = e.DOB,
                    RoleName = e.Role.RoleName,
                    DeptName = e.Department.DeptName,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            if (!employees.Any())
                return new PagedResponse<EmployeeResponse>(new List<EmployeeResponse>(), totalRecords, pageNumber, pageSize);

            return new PagedResponse<EmployeeResponse>(employees, totalRecords, pageNumber, pageSize);
        }


        public async Task<EmployeeResponse> UpdateEmployee(int Id, UpdateEmployee employeeDto)
        {
            var employee = _employeeRepo.Get(Id);
            if (employee == null)
                return null;

            employee.Lastname = employeeDto.Lastname;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;

            if (employeeDto.RoleId.HasValue)
                employee.RoleId = employeeDto.RoleId.Value;

            if (employeeDto.DepartmentId.HasValue)
                employee.DepartmentId = employeeDto.DepartmentId.Value;

            if (employeeDto.JobTitleId.HasValue)
                employee.JobTitleId = employeeDto.JobTitleId.Value;

            employee.UpdatedAt = DateTime.UtcNow;

            _employeeRepo.Update(employee);
            await _dbContext.SaveChangesAsync();

            _cache.Remove($"employee_{Id}");

            var updatedEmployee = await _employeeRepo.GetWithIncludesAsync( Id, e => e.Department, e => e.Role, e => e.JobTitle);

            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse> PatchEmployee(int id, PartialUpdate employeedto)
        {
            var employee = await _employeeRepo.GetWithIncludesAsync(id, e => e.Department, e => e.Role, e => e.JobTitle);

            if (employee == null)
                throw new Exception($"Employee with ID {id} not found");

            if (employeedto.IsActive.HasValue)
                employee.IsActive = employeedto.IsActive.Value;

            _employeeRepo.Update(employee);
            await _dbContext.SaveChangesAsync();

            // remove cache so next GetById fetches fresh from DB
            _cache.Remove($"employee_{id}");

            Log.Information("Removed cache for employee {EmployeeId} after patch", id);

            return _mapper.Map<EmployeeResponse>(employee);

        }
    }
}
