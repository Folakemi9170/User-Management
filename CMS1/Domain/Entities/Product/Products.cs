namespace UserManagement.Domain.Entities.Product
{
    public class Products : BaseEntity
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public decimal? Price { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? CreatedById { get; set; }
        public Employee CreatedBy { get; set; }

        public int? AssignedToId { get; set; }
        public Employee AssignedTo { get; set; }

        public int? CustomerId { get; set; }
        public Customers Customer { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public int ProductStatusId { get; set; }
        public ProductStatus ProductStatus { get; set; }

        public int ProductTypeId { get; set; }  // Digital Service, Digital Good, Physical Resource
        public ProductType ProductType { get; set; }

        public string? AvailabilityMessage { get; set; }

    }
}
