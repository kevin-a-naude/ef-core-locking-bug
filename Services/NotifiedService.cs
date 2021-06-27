using EfCoreBugDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EfCoreBugDemo.Services
{
    class NotifiedService
    {
        private readonly IDbContextFactory<ShopContext> dbContextFactory;

        public NotifiedService(IDbContextFactory<ShopContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task NotifyChange()
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            dbContext.Products.Add(new Product()
            {
                Name = "Nice",
                CategoryId = 1 // Nice Footwear
            });

            // SaveChangesAsync should not time out.
            // The above update does not interact with the change made in UpdateCategoryService.
            await dbContext.SaveChangesAsync();
        }
    }
}
