using UserManagement.Application.DTO.JobTitleDto;

namespace UserManagement.Application.Interfaces
{
    public interface IJobTitleService
    {
        Task<JobTitleResponse> CreateJobTitle(CreateJobTitle dto);
        Task<List<JobTitleResponse>> GetAllTitles();
        Task<IEnumerable<JobTitleResponse>> GetJobTitlesByDepartment(int departmentId);

    }
}
