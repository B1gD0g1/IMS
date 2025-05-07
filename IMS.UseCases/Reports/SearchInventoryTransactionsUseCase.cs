using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
    public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionsUseCase
    {
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;

        public SearchInventoryTransactionsUseCase(IInventoryTransactionRepository inventoryTransactionRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(
                string inventoryName,
                DateTime? startDate,
                DateTime? endDate,
                InventoryTransactionType? transactionType
            )
        {
            if (endDate.HasValue) endDate = endDate.Value.AddDays(1);

            return await this.inventoryTransactionRepository.GetInventoryTransactionsAsync(
                inventoryName,
                startDate,
                endDate,
                transactionType
            );
        }
    }
}
