using System.Data;

namespace UserManagement.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string? Description { get; set; }

        public ICollection<Roles> Roles { get; set; } = new List<Roles>();

    }
}