namespace UserManagement.Domain.Entities.Product
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
