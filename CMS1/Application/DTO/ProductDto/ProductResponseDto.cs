namespace UserManagement.Application.DTO.ProductDto
{
    public class ProductResponseDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
        public decimal? Price { get; set; }

        public string DepartmentName { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CustomerName { get; set; }

        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string ProductStatus { get; set; }

        public string AvailabilityMessage { get; set; }
    }
}
