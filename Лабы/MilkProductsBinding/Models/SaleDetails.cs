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

        // Новое поле для ЛР-13
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SummaProduct { get; set; }

        [ForeignKey("IdProductDetailSale")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("IdSale")]
        public virtual Sale Sale { get; set; } = null!;

        // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА для ЛР-13

        /// <summary>
        /// Вычисляемое свойство: Стоимость позиции
        /// Формула: Quantity * UnitPrice
        /// Это основное вычисляемое свойство для DetailSale
        /// </summary>
        [NotMapped]
        public decimal SummaProductCalculated
        {
            get
            {
                return QuantityProduct * UnitPrice;
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Стоимость позиции через цену из Product
        /// Формула: Quantity * Product.priceProduct
        /// Альтернативный расчет через связанную таблицу Product
        /// </summary>
        [NotMapped]
        public decimal SummaProductFromProductPrice
        {
            get
            {
                if (Product != null)
                {
                    return QuantityProduct * Product.priceProduct;
                }
                return 0;
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Экономия при покупке (если UnitPrice != Product.priceProduct)
        /// </summary>
        [NotMapped]
        public decimal PriceDifference
        {
            get
            {
                if (Product != null)
                {
                    return (Product.priceProduct - UnitPrice) * QuantityProduct;
                }
                return 0;
            }
        }
    }
}