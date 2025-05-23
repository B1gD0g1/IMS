﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository inventoryRepository;
        public List<InventoryTransaction> _inventoryTransactions = new List<InventoryTransaction>();

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? startDate, DateTime? endDate, InventoryTransactionType? transactionType)
        {
            var inventories = (await inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

            var query = from it in this._inventoryTransactions
                        join inv in inventories on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) ||
                            inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                            &&
                            (!startDate.HasValue || it.TransactionDate >= startDate.Value.Date) &&
                            (!endDate.HasValue || it.TransactionDate <= endDate.Value.Date) &&
                            (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new InventoryTransaction
                        {
                            Inventory = inv,
                            InventoryTransactionId = it.InventoryTransactionId,
                            PONumber = it.PONumber,
                            InventoryId = it.InventoryId,
                            QuantityBefore = it.QuantityBefore,
                            QuantityAfter = it.QuantityAfter,
                            ActivityType = it.ActivityType,
                            TransactionDate = it.TransactionDate,
                            DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice,
                        };

            return query;
        }

        public Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });

            return Task.CompletedTask;
        }

        public Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                PONumber = poNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventoy,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });

            return Task.CompletedTask;
        }
    }
}
