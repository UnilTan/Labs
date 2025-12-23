using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkProductsBinding.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            SaleDetails = new HashSet<SaleDetails>();
        }

        [Key]
        [Column("ProductId")]
        public int idProduct { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ProductName")]
        public string nameProduct { get; set; } = null!;

        [Column("Price", TypeName = "decimal(10, 2)")]
        public decimal priceProduct { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        public int ExpiryDays { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; }
    }
}