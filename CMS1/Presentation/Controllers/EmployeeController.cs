using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.EmployeeDto.Paging;
using UserManagement.Application.Interfaces;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeesById(id);

            if (employee == null) 
                return NotFound($"Employee with ID {id} not found.");

            return Ok(employee);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Register([FromBody] CreateEmployee dto)
        {
            var result = await _employeeService.CreateEmployee(dto);

            if (result == null)
                return BadRequest("Employee not found.");

            return Ok(result);
        }


        [HttpGet]
        [HasPermission("Employee View")]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetAll([FromQuery] Filter filter, [FromQuery] Pagination pagination)
        {
            var employees = await _employeeService.GetAllEmployees(filter, pagination);
            return Ok(employees);
        }


        [HttpPut("{Id}")]
        [HasPermission("Employee Update")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(int Id, [FromBody] UpdateEmployee employee)
        {
            try
            {
                var result = await _employeeService.UpdateEmployee(Id, employee);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{Id}")]
        [HasPermission("Employee Deactivation")]
        public async Task<ActionResult<EmployeeResponse>> PatchEmployee(int Id, [FromBody] PartialUpdate employeedto)
        {
            try
            {
                var result = await _employeeService.PatchEmployee(Id, employeedto);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
