using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.PermissionDto;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UMSDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Permission> _permissionRepo;

        public PermissionService(UMSDbContext dbContext, IMapper mapper, IGenericRepository<Permission> permissionRepo) 
        {   _dbContext = dbContext;
            _mapper = mapper;
            _permissionRepo = permissionRepo;
        }

        public async Task<PermissionResponse> CreatePermission(CreatePermissionDto dto)
        {
            bool exists = await _dbContext.Permissions.AnyAsync(p => p.SubModuleName == dto.SubModuleName);
            if (exists)
                throw new ArgumentException("Permission with this name already exists.");

            var permission = _mapper.Map<Permission>(dto);
           
            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<PermissionResponse>(permission);
            return response;
        }

        public async Task<PermissionResponseDto> GetAllPermission()
        {
            var permissions = await _permissionRepo.GetAll();
            if (permissions == null || !permissions.Any())
            {
                return new PermissionResponseDto
                {
                    Message = "No permission found",
                    Permissions = new List<PermissionResponse>()
                };
            }

            return new PermissionResponseDto
            {
                Message = "Permission retrieved successfully",
                Permissions = _mapper.Map<IEnumerable<PermissionResponse>>(permissions)
            };
        }

        public async Task<PermissionResponseDto> DeletePermission(int id)
        {
            var permission = (await _permissionRepo.GetByConditionAsync(p => p.Id == id));

            if (permission == null)
            {
                return new PermissionResponseDto
                {
                    Message = $"Permission with id {id} not found",
                    Permissions = new List<PermissionResponse>()
                };
            }

            _permissionRepo.Delete(id);
            await _dbContext.SaveChangesAsync();

            return new PermissionResponseDto
            {
                Message = "Permission deleted successfully",
                Permissions = new List<PermissionResponse>
                {
                    _mapper.Map<PermissionResponse>(permission)
                }
            };
        }
    }
}
