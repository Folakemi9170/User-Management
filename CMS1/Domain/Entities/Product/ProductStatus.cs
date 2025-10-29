namespace UserManagement.Domain.Entities.Product
{
    public class ProductStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Active";  // Active, Inactive, Discontinued for internal
        public string Description { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}