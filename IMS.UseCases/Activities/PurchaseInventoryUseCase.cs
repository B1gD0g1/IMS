using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;

        public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory,
            int quantity, string doneBy)
        {
            //在库存交易表中插入一条记录
            await inventoryTransactionRepository
                .PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

            //增加库存数量
            inventory.Quantity += quantity;
            await this.inventoryRepository.UpdateInventoryAsync(inventory);
        }
    }
}
