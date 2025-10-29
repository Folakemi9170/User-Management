namespace UserManagement.Domain.Entities.Product
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }   // Internal / External
        public string Description { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
