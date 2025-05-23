﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        private readonly IProductRepository productRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;

        public ProductTransactionRepository(
            IProductRepository productRepository, 
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //减少库存
            var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        //添加库存交易
                        await this.inventoryTransactionRepository.ProduceAsync(
                                 productionNumber,
                                 pi.Inventory,
                                 pi.InventoryQuantity * quantity,
                                 doneBy,
                                 -1);

                        //减少库存
                        var inv = await this.inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await this.inventoryRepository.UpdateInventoryAsync(inv);
                    }
                }
            }

            //添加产品交易
            this._productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy
            });

        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            this._productTransactions.Add(new ProductTransaction
            {
                SONumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.SellProduct,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? startDate, DateTime? endDate, ProductTransactionType? transactionType)
        {

            var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            var query = from pt in this._productTransactions
                        join pro in products on pt.ProductId equals pro.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) ||
                            pro.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                            &&
                            (!startDate.HasValue || pt.TransactionDate >= startDate.Value.Date) &&
                            (!endDate.HasValue || pt.TransactionDate <= endDate.Value.Date) &&
                            (!transactionType.HasValue || pt.ActivityType == transactionType)
                        select new ProductTransaction
                        {
                            Product = pro,
                            ProductTransactionId = pt.ProductTransactionId,
                            SONumber = pt.SONumber,
                            ProductionNumber = pt.ProductionNumber,
                            ProductId = pt.ProductId,
                            QuantityBefore = pt.QuantityBefore,
                            QuantityAfter = pt.QuantityAfter,
                            ActivityType = pt.ActivityType,
                            TransactionDate = pt.TransactionDate,
                            DoneBy = pt.DoneBy,
                            UnitPrice = pt.UnitPrice,
                        };

            return query;
        }
    }
}
