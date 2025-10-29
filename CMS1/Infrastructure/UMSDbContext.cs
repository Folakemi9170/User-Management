using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Entities.Product;

namespace UserManagement.Infrastructure
{
    public class UMSDbContext: DbContext
    {
        public UMSDbContext(DbContextOptions<UMSDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Customers> Customer { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<ProductStatus> ProductStatus { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<User>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.JobTitle)
                .WithMany()
                .HasForeignKey(e => e.JobTitleId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Employee>()
               .HasOne(e => e.Department)
               .WithMany(d => d.Employees)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Employee>()
               .HasOne(e => e.Role)
                .WithMany((r => r.Employees))
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<JobTitle>()
                .HasOne(j => j.Department)
                .WithMany(d => d.JobTitles)
                .HasForeignKey(j => j.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<JobTitle>()
               .HasOne(j => j.Role)
               .WithMany(r => r.JobTitles)
               .HasForeignKey(j => j.RoleId);
            modelBuilder.Entity<JobTitle>()
                .Property(j => j.MinimumSalary)
                .HasPrecision(18, 2);
            modelBuilder.Entity<JobTitle>()
                .Property(j => j.MaximumSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Products>()
                .HasOne(p => p.CreatedBy)
                .WithMany(e => e.CreatedProducts)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Products>()
               .HasOne(p => p.AssignedTo)
               .WithMany(e => e.AssignedProducts)
               .HasForeignKey(p => p.AssignedToId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Products>()
               .HasOne(p => p.Customer)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}
