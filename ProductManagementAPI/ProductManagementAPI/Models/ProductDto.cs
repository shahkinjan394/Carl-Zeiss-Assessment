using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Models
{
    public class ProductDto
    {
        [Key]
        [StringLength(6)]
        public string ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockAvailable { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
