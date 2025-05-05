using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseViewModel
    {
        [Required]
        public string PONumber { get; set; } = string.Empty;

        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "必须选择一个库存")]
        public int InventoryId { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "数量必须大于或等于1")]
        public int QuantityToPurchase { get; set; }

        public double InventoryPrice { get; set; }
    }
}
