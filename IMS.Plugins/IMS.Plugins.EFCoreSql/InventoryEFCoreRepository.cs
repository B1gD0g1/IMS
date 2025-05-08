using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSql
{
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public InventoryEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Inventories?.Add(inventory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteInventoryByIdAsync(int inventoryId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var inventory = db.Inventories?.Find(inventoryId);
            if (inventory is null) return;

            db.Inventories?.Remove(inventory);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            return await db.Inventories
                .Where(x => x.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var inventory = await db.Inventories.FindAsync(inventoryId);
            if (inventory is not null) return inventory;

            return new Inventory();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            using var db = this.contextFactory.CreateDbContext();
            var inventoryToUpdate = await db.Inventories.FindAsync(inventory.InventoryId);
            if (inventoryToUpdate is not null)
            {
                inventoryToUpdate.InventoryName = inventory.InventoryName;
                inventoryToUpdate.Price = inventory.Price;
                inventoryToUpdate.Quantity = inventory.Quantity;

                await db.SaveChangesAsync();
            }
        }
    }
}
