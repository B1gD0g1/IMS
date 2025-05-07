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
    public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
    {
        private readonly IProductTransactionRepository inventoryTransactionRepository;

        public SearchProductTransactionsUseCase(IProductTransactionRepository inventoryTransactionRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(
                string inventoryName,
                DateTime? startDate,
                DateTime? endDate,
                ProductTransactionType? transactionType
            )
        {
            if (endDate.HasValue) endDate = endDate.Value.AddDays(1);

            return await this.inventoryTransactionRepository.GetProductTransactionsAsync(
                inventoryName,
                startDate,
                endDate,
                transactionType
            );
        }
    }
}
