﻿using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSql
{
    public class IMSContext : DbContext
    {
        public IMSContext(DbContextOptions<IMSContext> options) : base(options)
        {
            
        }

        public DbSet<Inventory>? Inventories { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductInventory>? ProductInventories { get; set; }
        public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
        public DbSet<ProductTransaction>? ProductTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInventory>()
                .HasKey(pi => new { pi.ProductId, pi.InventoryId });

            modelBuilder.Entity<ProductInventory>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductInventories)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductInventory>()
                .HasOne(pi => pi.Inventory)
                .WithMany(i => i.ProductInventories)
                .HasForeignKey(pi => pi.InventoryId);

            //seeding data
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, InventoryName = "自行车座椅", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 2, InventoryName = "自行车身", Quantity = 10, Price = 15 },
                new Inventory { InventoryId = 3, InventoryName = "自行车轮子", Quantity = 20, Price = 8 },
                new Inventory { InventoryId = 4, InventoryName = "自行车踏板", Quantity = 20, Price = 1 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "自行车", Quantity = 10, Price = 150 },
                new Product { ProductId = 2, ProductName = "汽车", Quantity = 5, Price = 25000 }
            );

            modelBuilder.Entity<ProductInventory>().HasData(
                new ProductInventory { ProductId = 1, InventoryId = 1, InventoryQuantity = 1 },//自行车座椅
                new ProductInventory { ProductId = 1, InventoryId = 2, InventoryQuantity = 1 },//自行车身
                new ProductInventory { ProductId = 1, InventoryId = 3, InventoryQuantity = 2 },//自行车轮子
                new ProductInventory { ProductId = 1, InventoryId = 4, InventoryQuantity = 2 } //自行车踏板
            );
        }
    }
}
