using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MilkProductsBinding.Models
{
    [Table("Sale")]
    public partial class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetails>();
        }

        [Key]
        [Column("SaleId")]
        public int IdSale { get; set; }

        public DateTime SaleDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        // Новые поля для ЛР-13
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SummaSale { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? DiscountSale { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Resultat { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; }

        // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА для ЛР-13

        /// <summary>
        /// Вычисляемое свойство: Сумма всех продуктов в данной продаже
        /// Формула: SUM(Quantity * UnitPrice) для всех SaleDetails этой продажи
        /// </summary>
        [NotMapped]
        public decimal SummaSaleCalculated
        {
            get
            {
                if (SaleDetails != null && SaleDetails.Any())
                {
                    return SaleDetails.Sum(detail => detail.QuantityProduct * detail.UnitPrice);
                }
                return 0;
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Итоговая сумма с учетом скидки
        /// Формула: SummaSale - (SummaSale * DiscountSale / 100)
        /// </summary>
        [NotMapped]
        public decimal ResultatCalculated
        {
            get
            {
                decimal summa = SummaSaleCalculated;
                decimal discount = DiscountSale ?? 0;
                return summa - (summa * discount / 100);
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Размер скидки в рублях
        /// </summary>
        [NotMapped]
        public decimal DiscountAmount
        {
            get
            {
                decimal summa = SummaSaleCalculated;
                decimal discount = DiscountSale ?? 0;
                return summa * discount / 100;
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Количество позиций в продаже
        /// </summary>
        [NotMapped]
        public int ItemsCount
        {
            get
            {
                return SaleDetails?.Count ?? 0;
            }
        }
    }
}