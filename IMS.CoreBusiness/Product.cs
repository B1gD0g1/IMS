using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "数量必须大于或等于零。")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "单价必须大于或等于零。")]
        public double Price { get; set; }

        [Product_EnsurePriceIsGreaterThanInventoriesCost]
        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();


        public void AddInventory(Inventory inventory)
        {
            if (!this.ProductInventories.Any(
                x => x.Inventory is not null &&
                x.Inventory.InventoryName.Equals(inventory.InventoryName)))
            {
                this.ProductInventories.Add(new ProductInventory
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    InventoryQuantity = 1,
                    ProductId = this.ProductId,
                    Product = this
                });
            }
        }

        public void RemoveInventory(ProductInventory productInventory)
        {
            this.ProductInventories?.Remove(productInventory);
        }
    }
}
