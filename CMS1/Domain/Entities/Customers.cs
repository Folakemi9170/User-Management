using System.Net.Mail;
using UserManagement.Domain.Entities.Product;

namespace UserManagement.Domain.Entities
{
    public class Customers : BaseEntity
    {
        public string Firstname { get; set; }
        public string? Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string ContactAddress { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Products> Products { get; set; }

    }
}
