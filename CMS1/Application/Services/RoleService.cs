using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.Application.DTO.PermissionDto;
using UserManagement.Application.DTO.Roles;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly UMSDbContext _dbContext;
        private readonly IMapper _mapper;

        public RoleService(UMSDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<RoleResponseDto> CreateRole(CreateRoleDto dto)
        {
            var exists = await _dbContext.Roles
                .AnyAsync(r => r.RoleName.ToLower() == dto.RoleName.ToLower());
            if (exists)
                throw new ArgumentException($"Roles '{dto.RoleName}' already exists.");

            var role = _mapper.Map<Roles>(dto);
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<List<PermissionRoleResponse>> GetAllRoles()
        {
            var roles = await _dbContext.Roles
                .Include(r => r.Permissions) 
                .ToListAsync();

            var result = roles.Select(role => new PermissionRoleResponse
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                Permissions = role.Permissions.Select(p => new PermissionResponse
                {
                    Id = p.Id,
                    ModuleName = p.ModuleName,
                    SubModuleName = p.SubModuleName,
                    Description = p.Description
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<PermissionRoleResponse>  GetPermissionByRoleId(int roleId)
        {
            var role = await _dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");


            var result = new PermissionRoleResponse
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                Permissions = role.Permissions.Select(p => new PermissionResponse
                {
                    Id = p.Id,
                    ModuleName = p.ModuleName,
                    SubModuleName = p.SubModuleName,
                    Description = p.Description
                }).ToList()
            };

            return result;
        }

        public async Task AssignPermissionsToRole(int roleId, List<int> permissionIds)
        {
            var role = await _dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }

            var permissions = await _dbContext.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .ToListAsync();
            if (permissions.Count != permissionIds.Count)
            {
                throw new KeyNotFoundException("One or more permissions not found.");
            }
            foreach (var permission in permissions)
            {
                if (!role.Permissions.Contains(permission))
                {
                    role.Permissions.Add(permission);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PermissionRoleResponse> UpdateRolePermission(int Id, UpdateRole dto)
        {
            var role = await _dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == Id);

            if (role == null)
                throw new KeyNotFoundException($"Role with ID {Id} not found.");

            if (!string.IsNullOrWhiteSpace(dto.RoleName) && dto.RoleName != role.RoleName)
            {
                bool nameExists = await _dbContext.Roles
                    .AnyAsync(r => r.RoleName == dto.RoleName && r.Id != Id);
                if (nameExists)
                    throw new ApplicationException($"A role with the name '{dto.RoleName}' already exists.");

                role.RoleName = dto.RoleName;
            }

            if (dto.Description != null)
                role.Description = dto.Description;

            if (dto.PermissionId?.Any() == true)
            {
                var permissions = await _dbContext.Permissions
                    .Where(p => dto.PermissionId.Contains(p.Id))
                    .ToListAsync();

                if (permissions.Count != dto.PermissionId.Count)
                {
                    var missingIds = dto.PermissionId.Except(permissions.Select(p => p.Id));
                    throw new KeyNotFoundException($"Permissions not found: {string.Join(", ", missingIds)}");
                }

                role.Permissions = permissions;
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PermissionRoleResponse>(role);
        }
    }
}
