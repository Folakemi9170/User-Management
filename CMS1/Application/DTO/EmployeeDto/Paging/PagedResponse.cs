namespace UserManagement.Application.DTO.EmployeeDto.Paging
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }


        public PagedResponse(IEnumerable<T> data, int totalRecords, int pageNumber, int pageSize)
        {
            Data = data ?? new List<T>();
            TotalRecords = totalRecords;
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 10 : pageSize;
            TotalPages = (int)Math.Ceiling(TotalRecords / (double)PageSize);
        }
    }
}
