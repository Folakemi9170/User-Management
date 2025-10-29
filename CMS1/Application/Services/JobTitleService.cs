using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.JobTitleDto;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly UMSDbContext _dbContext;
        private readonly IGenericRepository<JobTitle> _repo;
        private readonly IMapper _mapper;

        public JobTitleService(UMSDbContext dbContext, IGenericRepository<JobTitle> repo, IMapper mapper)
        {
            _dbContext = dbContext;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<JobTitleResponse> CreateJobTitle(CreateJobTitle dto)
        {
            var jobTitle = _mapper.Map<JobTitle>(dto);

            var roleExists = await _dbContext.Roles.AnyAsync(r => r.Id == jobTitle.RoleId);
            var deptExists = await _dbContext.Departments.AnyAsync(d => d.Id == jobTitle.DepartmentId);

            if (!roleExists || !deptExists)
                throw new Exception("Invalid Role or Department");

            await _dbContext.AddAsync(jobTitle);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(jobTitle).Reference(j => j.Role).LoadAsync();
            await _dbContext.Entry(jobTitle).Reference(j => j.Department).LoadAsync();

            return _mapper.Map<JobTitleResponse>(jobTitle);
        }

        public async Task<List<JobTitleResponse>> GetAllTitles()
        {
            var titles = await _dbContext.JobTitles
                .Include(d => d.Department)
                .ToListAsync();

            return _mapper.Map<List<JobTitleResponse>>(titles);
        }

        public async Task<IEnumerable<JobTitleResponse>> GetJobTitlesByDepartment(int departmentId)
        {
            var jobTitles = await _dbContext.JobTitles
                .Include(j => j.Department)
                .Include(j => j.Role)
                .Where(j => j.DepartmentId == departmentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobTitleResponse>>(jobTitles);
        }
    }
}
