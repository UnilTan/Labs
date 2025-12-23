using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkProductsCatalog.Models
{
    [Table("Sale")]
    public partial class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetails>();
        }

        [Key]
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; }
    }
}