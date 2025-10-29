using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.Interfaces;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]

    public class DeptController : ControllerBase
    {
        private readonly IDeptService _deptService;
        public DeptController(IDeptService deptService)
        {
            _deptService = deptService;
        }

        [HttpGet]
        [HasPermission("Department View")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _deptService.GetAllDepartments();

            if (!departments.Departments.Any())
                return NotFound(departments); ;

            return Ok(departments);
        }

        [HttpPost]
        [HasPermission("Department Creation")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDeptDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DeptName))
                return BadRequest("Department name is required.");

            var result = await _deptService.CreateDepartment(dto);
            return CreatedAtAction(nameof(GetDepartments), new { name = result.DeptName }, result);
        }

        [HttpGet("{id}")]
        [HasPermission("Department View")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var result = await _deptService.GetDepartment(id);

            if (result.Departments == null || !result.Departments.Any())
                return NotFound(result);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [HasPermission("Department Deletion")]
        public async Task<ActionResult<DepartmentResponseDto>> DeleteDepartment(int id)
        {
            var department = await _deptService.Delete(id);
            if (department.Departments == null || !department.Departments.Any())
                return NotFound(department);

            return Ok(department);
        }
    }
}
