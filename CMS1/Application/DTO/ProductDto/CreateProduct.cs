namespace UserManagement.Application.DTO.ProductDto
{
    public class CreateProduct
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal? Price { get; set; }
        public int ProductTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CreatedById { get; set; }
        public int? AssignedToId { get; set; }
        public int? CustomerId { get; set; }
        public int ProductStatusId { get; set; } = 1;
    }
}
