using EfCoreBugDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EfCoreBugDemo.Services
{
    class UpdateCategoryService
    {
        private readonly IDbContextFactory<ShopContext> dbContextFactory;

        public UpdateCategoryService(IDbContextFactory<ShopContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task ExecuteAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var category = await dbContext.Categories.Where(item => item.Name == "Footwear").SingleAsync();

            category.Discontinued = !category.Discontinued;
            await dbContext.SaveChangesAsync();

            // NotifiedService should not cause our transaction to time out.
            var service = new NotifiedService(dbContextFactory);
            await service.NotifyChange();

            await transaction.CommitAsync();
        }
    }
}
