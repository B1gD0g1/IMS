using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(150)]
        public string InventoryName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "数量必须大于或等于零。")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "单价必须大于或等于零。")]
        public double Price { get; set; }
    }
}
