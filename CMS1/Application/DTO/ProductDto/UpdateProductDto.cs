namespace UserManagement.Application.DTO.ProductDto
{
    public class UpdateProductDto
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? Price { get; set; }
        public int? ProductStatusId { get; set; }
        public int? ProductTypeId { get; set; }
    }
}
