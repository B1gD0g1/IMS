using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.Interfaces
{
    public interface ISearchProductTransactionsUseCase
    {
        Task<IEnumerable<ProductTransaction>> ExecuteAsync(string inventoryName, DateTime? startDate, DateTime? endDate, ProductTransactionType? transactionType);
    }
}