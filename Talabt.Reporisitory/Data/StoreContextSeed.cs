using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;
using Talabt.Reporisitory.Data;

namespace Talabt.Reporisitory
{
    public static class StoreContextSeed
    {
        public static async Task GetDataSeedkAsync(StoreContext dbContext)
        {
            var seedDirectory = Path.Combine("..", "Talabt.Reporisitory", "Data", "DataSeed");

            await SeedDataFromJsonAsync<ProductBrand>(dbContext, Path.Combine(seedDirectory, "brands.json"));
            await SeedDataFromJsonAsync<ProductType>(dbContext, Path.Combine(seedDirectory, "types.json"));
            await SeedDataFromJsonAsync<Product>(dbContext, Path.Combine(seedDirectory, "products.json"));
            await SeedDataFromJsonAsync<DeliveryMethod>(dbContext, Path.Combine(seedDirectory, "delivery.json"));
        }
        public static async Task SeedDataFromJsonAsync<T>(StoreContext dbContext, string filePath) where T : class
        {
            if (!await dbContext.Set<T>().AnyAsync())
            {
                var data = File.ReadAllText(filePath);
                var items = JsonSerializer.Deserialize<List<T>>(data);
                if (items?.Count > 0)
                {
                    foreach (var item in items)
                        await dbContext.Set<T>().AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
