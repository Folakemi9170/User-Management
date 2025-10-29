using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities.Product;
using UserManagement.Infrastructure;

namespace UserManagement.Utility
{
    public class AvailabilityMessage
    {
        private readonly UMSDbContext _dbContext;
        public AvailabilityMessage(UMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetAvailabilityMessage(Products product)
        {
            bool isInternal = product.ProductCategoryId == 1;
            bool isExternal = product.ProductCategoryId == 2;
            bool isActive = product.ProductStatusId == 1;
            bool isInactive = product.ProductStatusId == 2;
            bool isDiscontinued = product.ProductStatusId == 3;

            if (isInternal)
            {
                product.CustomerId = null; //not 0
            }
            else if (isExternal)
            {
                if (product.CustomerId == null)
                    throw new ArgumentException("External products must have a valid customer ID.");


                bool customerExists = await _dbContext.Customer
                    .AnyAsync(c => c.Id == product.CustomerId.Value);

                if (!customerExists)
                    throw new ArgumentException($"Customer with ID {product.CustomerId} does not exist.");
            }

            string message = isExternal && isActive ? "For sale" :
                             isExternal && isInactive ? "Product unavailable" :
                             isExternal && isDiscontinued ? "Product unavailable" :
                             isInternal && isActive ? "Available for organizational use only" :
                             isInternal && isInactive ? "Product temporarily unavailable" :
                             isInternal && isDiscontinued ? "This product is not available for use" :
                             "Status unknown";
            return message;
        }
    }
}
