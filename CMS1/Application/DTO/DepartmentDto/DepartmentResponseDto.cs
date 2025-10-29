namespace UserManagement.Application.DTO.DepartmentDto
{
    public class DepartmentResponseDto
    {
        public string Message { get; set; }
        public IEnumerable<ResponseDto> Departments { get; set; }
    }
}
