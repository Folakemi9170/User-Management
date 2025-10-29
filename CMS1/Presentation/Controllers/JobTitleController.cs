using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.JobTitleDto;
using UserManagement.Application.DTO.Roles;
using UserManagement.Application.Interfaces;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleController : ControllerBase
        {
        private readonly IJobTitleService _jobTitleService;
        public JobTitleController(IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        
        [HttpPost]
        public async Task<ActionResult<RoleResponseDto>> CreateRole([FromBody] CreateJobTitle dto)
        {
            var role = await _jobTitleService.CreateJobTitle(dto);

            return CreatedAtAction(nameof(GetAllRoles), role);
        }

        [HttpGet]
        public async Task<ActionResult<List<JobTitleResponse>>> GetAllRoles()
        {
            var titles = await _jobTitleService.GetAllTitles();
            return Ok(titles);
        }

        [HttpGet("{departmentId}")]
        public async Task<ActionResult<IEnumerable<JobTitleResponse>>> GetJobTitlesByDepartment(int departmentId)
        {
            var jobTitles = await _jobTitleService.GetJobTitlesByDepartment(departmentId);
            return Ok(jobTitles);
        }
    }
}
