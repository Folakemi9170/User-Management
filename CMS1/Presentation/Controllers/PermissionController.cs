using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.PermissionDto;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetPermission()
        {
            var permission = await _permissionService.GetAllPermission();

            if (!permission.Permissions.Any())
                return NotFound(permission); ;

            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionResponse>> Create(CreatePermissionDto dto)
        {
            var permission = await _permissionService.CreatePermission(dto);

            if (permission == null) 
                return BadRequest("Failed to create permission");

            return Ok(permission);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PermissionResponseDto>> DeletePermission(int id)
        {
            var permission = await _permissionService.DeletePermission(id);
            if (permission.Permissions == null || !permission.Permissions.Any())
                return NotFound(permission);

            return Ok(permission);
        }
    }
}
