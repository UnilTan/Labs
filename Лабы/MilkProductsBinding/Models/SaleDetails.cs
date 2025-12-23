using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkProductsBinding.Models
{
    [Table("SaleDetails")]
    public partial class SaleDetails
    {
        [Key]
        [Column("SaleDetailId")]
        public int IdDetailSale { get; set; }

        [Column("SaleId")]
        public int IdSale { get; set; }

        [Column("ProductId")]
        public int IdProductDetailSale { get; set; }

        [Column("Quantity")]
        public int QuantityProduct { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("IdProductDetailSale")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("IdSale")]
        public virtual Sale Sale { get; set; } = null!;
    }
}