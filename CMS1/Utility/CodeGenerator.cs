using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Entities.Product;
using UserManagement.Infrastructure;

namespace UserManagement.Utility
{
    public static class CodeGenerator
    {
        /// <summary>
        /// Generates a unique product code with the pattern: 
        /// #XXdd-nn (e.g., #TE01-01) for departmental products
        /// #GEN00-nn (e.g., #GEN00-05) for non-departmental products
        /// </summary>
        public static async Task<string> GenerateProductCodeAsync(UMSDbContext dbContext, int? departmentId)
        {
            string deptPrefix = "GEN";
            string deptIdPart = "00";

            if (departmentId.HasValue)
            {
                var department = await dbContext.Departments
                    .Where(d => d.Id == departmentId.Value)
                    .Select(d => d.DeptName)
                    .FirstOrDefaultAsync();

                if (department != null)
                {
                    deptPrefix = department.Length >= 2
                        ? department.Substring(0, 2).ToUpper()
                        : department.ToUpper();
                    deptIdPart = departmentId.Value.ToString("D2");
                }
            }

            // Get the highest existing sequence number for this prefix
            string prefixPattern = $"#{deptPrefix}{deptIdPart}-";

            var lastProduct = await dbContext.Product
                .Where(p => p.ProductCode.StartsWith(prefixPattern))
                .OrderByDescending(p => p.ProductCode)
                .Select(p => p.ProductCode)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastProduct != null)
            {
                // Extract the sequence number from the last code
                var parts = lastProduct.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefixPattern}{nextNumber:D2}";
        }

        /// <summary>
        /// Generates a general-purpose code for any entity, e.g., Employee, Customer, etc.
        /// Pattern: PREFIX-yyyyMMdd-nn (e.g., EMP-20251006-01)
        /// </summary>
        public static async Task<string> GenerateGenericCodeAsync(UMSDbContext dbContext, string tableName, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Prefix cannot be null or empty.", nameof(prefix));

            int count = tableName switch
            {
                "Employees" => await dbContext.Employees.CountAsync(),
                "Customers" => await dbContext.Customer.CountAsync(),
                "Products" => await dbContext.Product.CountAsync(),
                _ => throw new ArgumentException($"Unsupported table name: {tableName}")
            };

            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            int next = count + 1;

            return $"{prefix}-{datePart}-{next:D2}";
        }
    }
}
