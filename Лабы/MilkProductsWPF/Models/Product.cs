using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkProductsWPF.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            SaleDetails = new HashSet<SaleDetails>();
        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        public int ExpiryDays { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City? City { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; }
    }
}