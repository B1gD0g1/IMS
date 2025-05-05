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
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;
        private readonly IProductRepository productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository,
            IProductRepository productRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
            this.productRepository = productRepository;
        }


        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //添加产品交易记录
            //添加库存交易记录
            //减少库存数量
            await this.productTransactionRepository.ProduceAsync(productionNumber, product, quantity, doneBy);

            //更新产品数量
            product.Quantity += quantity;
            await this.productRepository.UpdateProductAsync(product);
        }
    }
}
