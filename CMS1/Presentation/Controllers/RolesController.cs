
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.Roles;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult<RoleResponseDto>> CreateRole([FromBody] CreateRoleDto dto)
        {
            var role = await _roleService.CreateRole(dto);

            return CreatedAtAction(nameof(GetAllRoles), role);
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionRoleResponse>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost("{roleId}/assign-permissions")]
        public async Task<IActionResult> AssignPermissionToRole(int roleId, [FromBody] List<int> permissionIds)
        {
            try
            {
                await _roleService.AssignPermissionsToRole(roleId, permissionIds);
                return Ok(new { message = "Permissions assigned successfully" });
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> GetPermissionByRoleId(int roleId)
        {
            try
            {
                var role = await _roleService.GetPermissionByRoleId(roleId);
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<PermissionRoleResponse>> UpdateRolePermission(int Id, UpdateRole dto)
        {
            try
            {
                var role = await _roleService.UpdateRolePermission(Id, dto);
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
